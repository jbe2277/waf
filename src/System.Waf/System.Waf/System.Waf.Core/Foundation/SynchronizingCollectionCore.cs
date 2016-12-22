using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace System.Waf.Foundation
{
    /// <summary>
    /// Represents a collection that synchronizes all of it's items with the items of the specified original collection.
    /// When the original collection notifies a change via the <see cref="INotifyCollectionChanged"/> interface then
    /// this collection synchronizes it's own items with the original items.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <typeparam name="TOriginal">The type of elements in the original collection.</typeparam>
    public class SynchronizingCollectionCore<T, TOriginal> : ReadOnlyObservableList<T>, IDisposable
    {
        private readonly ObservableCollection<T> innerCollection;
        private readonly List<Tuple<TOriginal, T>> mapping;
        private readonly IEnumerable<TOriginal> originalCollection;
        private readonly INotifyCollectionChanged originalObservableCollection;
        private readonly Func<TOriginal, T> factory;
        private readonly IEqualityComparer<T> itemComparer;
        private readonly IEqualityComparer<TOriginal> originalItemComparer;
        private volatile bool isDisposed;


        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizingCollectionCore{T, TOriginal}"/> class.
        /// </summary>
        /// <param name="originalCollection">The original collection.</param>
        /// <param name="factory">The factory which is used to create new elements in this collection.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        /// <exception cref="ArgumentNullException">The argument factory must not be null.</exception>
        public SynchronizingCollectionCore(IEnumerable<TOriginal> originalCollection, Func<TOriginal, T> factory)
            : this(originalCollection, factory, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizingCollectionCore{T, TOriginal}"/> class.
        /// </summary>
        /// <param name="originalCollection">The original collection.</param>
        /// <param name="factory">The factory which is used to create new elements in this collection.</param>
        /// <param name="noCollectionChangedHandler">Pass true when the subclass takes care about the collection changed event of the originalCollection. Default is false.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        /// <exception cref="ArgumentNullException">The argument factory must not be null.</exception>
        protected SynchronizingCollectionCore(IEnumerable<TOriginal> originalCollection, Func<TOriginal, T> factory, bool noCollectionChangedHandler)
            : base(new ObservableCollection<T>())
        {
            if (originalCollection == null) { throw new ArgumentNullException(nameof(originalCollection)); }
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }

            this.mapping = new List<Tuple<TOriginal, T>>();
            this.originalCollection = originalCollection;
            this.factory = factory;
            this.itemComparer = EqualityComparer<T>.Default;
            this.originalItemComparer = EqualityComparer<TOriginal>.Default;

            if (!noCollectionChangedHandler)
            {
                originalObservableCollection = originalCollection as INotifyCollectionChanged;
                if (originalObservableCollection != null)
                {
                    originalObservableCollection.CollectionChanged += OriginalCollectionChanged;
                }
            }

            innerCollection = (ObservableCollection<T>)Items;
            foreach (TOriginal item in originalCollection)
            {
                innerCollection.Add(CreateItem(item));
            }
        }


        /// <summary>
        /// Call this method when the orignal collection has changed.
        /// </summary>
        /// <param name="sender">The sender of the collection changed event.</param>
        /// <param name="e">The event args of the collection changed event.</param>
        /// <exception cref="ArgumentNullException">Argument e must not be null.</exception>
        protected void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e == null) { throw new ArgumentNullException(nameof(e)); }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex >= 0)
                {
                    int i = e.NewStartingIndex;
                    foreach (TOriginal item in e.NewItems)
                    {
                        innerCollection.Insert(i, CreateItem(item));
                        i++;
                    }
                }
                else
                {
                    foreach (TOriginal item in e.NewItems)
                    {
                        innerCollection.Add(CreateItem(item));
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldStartingIndex >= 0)
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        RemoveAtCore(e.OldStartingIndex);
                    }
                }
                else
                {
                    foreach (TOriginal item in e.OldItems)
                    {
                        RemoveCore(item);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                if (e.NewStartingIndex >= 0)
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        innerCollection[i + e.NewStartingIndex] = CreateItem((TOriginal)e.NewItems[i]);
                    }
                }
                else
                {
                    foreach (TOriginal item in e.OldItems) { RemoveCore(item); }
                    foreach (TOriginal item in e.NewItems) { innerCollection.Add(CreateItem(item)); }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    innerCollection.Move(e.OldStartingIndex + i, e.NewStartingIndex + i);
                }
            }
            else // Reset
            {
                ClearCore();
                foreach (TOriginal item in originalCollection)
                {
                    innerCollection.Add(CreateItem(item));
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Call this method from a subclass when you implement a finalizer (destructor).
        /// </summary>
        /// <param name="disposing">if true then dispose unmanaged and managed resources; otherwise dispose only unmanaged resources.</param>
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

        /// <summary>
        /// Override this method to free, release or reset any resources.
        /// </summary>
        /// <param name="disposing">if true then dispose unmanaged and managed resources; otherwise dispose only unmanaged resources.</param>
        protected virtual void OnDispose(bool disposing)
        {
        }

        private T CreateItem(TOriginal oldItem)
        {
            T newItem = factory(oldItem);
            mapping.Add(new Tuple<TOriginal, T>(oldItem, newItem));
            return newItem;
        }

        private void RemoveCore(TOriginal oldItem)
        {
            Tuple<TOriginal, T> tuple = mapping.First(t => originalItemComparer.Equals(t.Item1, oldItem));
            mapping.Remove(tuple);
            innerCollection.Remove(tuple.Item2);
        }

        private void RemoveAtCore(int index)
        {
            T newItem = this[index];
            Tuple<TOriginal, T> tuple = mapping.First(t => itemComparer.Equals(t.Item2, newItem));
            mapping.Remove(tuple);
            innerCollection.RemoveAt(index);
        }

        private void ClearCore()
        {
            innerCollection.Clear();
            mapping.Clear();
        }
    }
}
