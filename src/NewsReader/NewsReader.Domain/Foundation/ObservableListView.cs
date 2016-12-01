using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Domain.Foundation
{
    public class ObservableListView<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyObservableList<T>
    {
        private const string indexerName = "Item[]";  // This must be equal to Binding.IndexerName
        private static readonly PropertyChangedEventArgs CountChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
        private static readonly PropertyChangedEventArgs IndexerChangedEventArgs = new PropertyChangedEventArgs(indexerName);

        private readonly IEnumerable<T> originalList;
        private readonly IEqualityComparer<T> comparer;
        private readonly INotifyCollectionChanged originalObservableCollection;
        private readonly List<T> innerList;
        private volatile bool isDisposed;
        private Predicate<T> filter;


        public ObservableListView(IEnumerable<T> originalList) : this(originalList, null)
        {
        }

        public ObservableListView(IEnumerable<T> originalList, IEqualityComparer<T> comparer) : base(new List<T>())
        {
            this.originalList = originalList;
            this.comparer = comparer ?? EqualityComparer<T>.Default;

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
            T[] newList;
            if (Filter != null)
            {
                newList = originalList.Where(x => Filter(x)).ToArray();
            }
            else
            {
                newList = originalList.ToArray();
            }

            ListMerger.Merge(newList, innerList, comparer, Insert, RemoveAt, () => Reset(newList));
        }

        private void Insert(int newItemIndex, T newItem)
        {
            innerList.Insert(newItemIndex, newItem);
            OnPropertyChanged(CountChangedEventArgs);
            OnPropertyChanged(IndexerChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, newItemIndex));
        }

        private void RemoveAt(int oldItemIndex)
        {
            var oldItem = innerList[oldItemIndex];
            innerList.RemoveAt(oldItemIndex);
            OnPropertyChanged(CountChangedEventArgs);
            OnPropertyChanged(IndexerChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, oldItemIndex));
        }

        private void Reset(IList<T> newList)
        {
            innerList.Clear();
            innerList.AddRange(newList);
            OnPropertyChanged(CountChangedEventArgs);
            OnPropertyChanged(IndexerChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateInnerList();
        }
    }
}
