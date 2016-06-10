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

            if (innerList.SequenceEqual(newList, comparer))
            {
                return;
            }

            // Item added or removed
            if (innerList.Count != newList.Length)
            {
                // Change of more than 1 item added or removed is not supported -> Reset
                if (Math.Abs(innerList.Count - newList.Length) != 1)
                {
                    Reset(newList);
                    return;
                }

                if (innerList.Count < newList.Length)
                {
                    int newItemIndex = -1;
                    for (int i = 0, j = 0; i < innerList.Count; i++, j++)
                    {
                        if (!comparer.Equals(innerList[i], newList[j]))
                        {
                            if (newItemIndex != -1)
                            {
                                // Second change is not supported -> Reset
                                Reset(newList);
                                return;
                            }
                            newItemIndex = j;
                            i--;
                        }
                    }
                    if (newItemIndex == -1)
                    {
                        newItemIndex = newList.Length - 1;
                    }
                    Insert(newItemIndex, newList[newItemIndex]);
                    return;
                }
                else
                {
                    int oldItemIndex = -1;
                    for (int i = 0, j = 0; i < newList.Length; i++, j++)
                    {
                        if (!comparer.Equals(innerList[i], newList[j]))
                        {
                            if (oldItemIndex != -1)
                            {
                                // Second change is not supported -> Reset
                                Reset(newList);
                                return;
                            }
                            oldItemIndex = i;
                            j--;
                        }
                    }
                    if (oldItemIndex == -1)
                    {
                        oldItemIndex = innerList.Count - 1;
                    }
                    RemoveAt(oldItemIndex);
                    return;
                }
            }
            
            Reset(newList);
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
