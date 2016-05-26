using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Applications.Services
{
    public class ObservableListView<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyObservableList<T>
    {
        private IEnumerable<T> originalList;
        private readonly INotifyCollectionChanged originalObservableCollection;
        private readonly List<T> innerList;
        private volatile bool isDisposed;
        private Predicate<T> filter;
        

        public ObservableListView(IEnumerable<T> originalList) : base(new List<T>())
        {
            this.originalList = originalList;

            originalObservableCollection = originalList as INotifyCollectionChanged;
            if (originalObservableCollection != null)
            {
                originalObservableCollection.CollectionChanged += OriginalCollectionChanged;
            }

            innerList = (List<T>)Items;
            innerList.AddRange(originalList);
        }


        public Predicate<T> Filter
        {
            get { return filter; }
            set
            {
                if (filter != value)
                {
                    filter = value;
                    UpdateInnerList();
                }
            }
        }


        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;


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

            OnDispose(disposing);
            if (disposing)
            {
                if (originalObservableCollection != null)
                {
                    originalObservableCollection.CollectionChanged -= OriginalCollectionChanged;
                }
            }
            isDisposed = true;
        }

        protected virtual void OnDispose(bool disposing)
        {
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void UpdateInnerList()
        {
            innerList.Clear();
            if (Filter != null)
            {
                innerList.AddRange(originalList.Where(x => Filter(x)));
            }
            else
            {
                innerList.AddRange(originalList);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        }

        private void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateInnerList();
        }
    }
}
