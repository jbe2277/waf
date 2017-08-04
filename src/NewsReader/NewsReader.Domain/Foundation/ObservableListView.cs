using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Jbe.NewsReader.Domain.Foundation
{
    public class ObservableListView<T> : ObservableListViewBase<T>, IDisposable
    {
        private readonly IEnumerable<T> originalList;
        private readonly IEqualityComparer<T> comparer;
        private readonly INotifyCollectionChanged originalObservableCollection;
        private Predicate<T> filter;
        private volatile bool isDisposed;


        public ObservableListView(IEnumerable<T> originalList) : this(originalList, null)
        {
        }

        public ObservableListView(IEnumerable<T> originalList, IEqualityComparer<T> comparer) : base(originalList)
        {
            this.originalList = originalList;
            this.comparer = comparer ?? EqualityComparer<T>.Default;

            originalObservableCollection = originalList as INotifyCollectionChanged;
            if (originalObservableCollection != null)
            {
                originalObservableCollection.CollectionChanged += OriginalCollectionChanged;
            }
        }


        public Predicate<T> Filter
        {
            get => filter;
            set
            {
                if (filter != value)
                {
                    filter = value;
                    UpdateInnerList();
                }
            }
        }


        public void Refresh()
        {
            UpdateInnerList();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (isDisposed) { return; }
            isDisposed = true;

            OnDispose(disposing);
            if (disposing)
            {
                if (originalObservableCollection != null)
                {
                    originalObservableCollection.CollectionChanged -= OriginalCollectionChanged;
                }
            }
        }

        protected virtual void OnDispose(bool disposing) { }
        
        private void UpdateInnerList()
        {
            T[] newList;
            if (Filter != null)
            {
                newList = originalList.Where(x => Filter(x)).ToArray();
            }
            else
            {
                newList = originalList.ToArray();
            }

            ListMerger.Merge(newList, InnerList, comparer, Insert, RemoveAt, () => Reset(newList));
        }

        private void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateInnerList();
        }
    }
}
