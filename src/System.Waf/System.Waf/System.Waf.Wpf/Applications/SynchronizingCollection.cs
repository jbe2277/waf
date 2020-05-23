using System.Collections.Generic;
using System.Collections.Specialized;
using System.Waf.Foundation;

namespace System.Waf.Applications
{
    /// <summary>
    /// Represents a collection that synchronizes all of it's items with the items of the specified original collection.
    /// When the original collection notifies a change via the <see cref="INotifyCollectionChanged"/> interface then
    /// this collection synchronizes it's own items with the original items.
    /// </summary>
    /// <remarks>
    /// In comparison to the base class this one uses weak event handlers to listen for collection changed events.
    /// Therefore, it does not produce a memory leak if Dispose is not called.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <typeparam name="TOriginal">The type of elements in the original collection.</typeparam>
    public class SynchronizingCollection<T, TOriginal> : SynchronizingCollectionCore<T, TOriginal>
    {
        private readonly IWeakEventProxy? originalCollectionChangedProxy;

        /// <summary>Initializes a new instance of the <see cref="SynchronizingCollection{T, TOriginal}"/> class.</summary>
        /// <param name="originalCollection">The original collection.</param>
        /// <param name="factory">The factory which is used to create new elements in this collection.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        /// <exception cref="ArgumentNullException">The argument factory must not be null.</exception>
        public SynchronizingCollection(IEnumerable<TOriginal> originalCollection, Func<TOriginal, T> factory)
            : base(originalCollection, factory, true)
        {
            if (originalCollection is INotifyCollectionChanged originalObservableCollection)
            {
                originalCollectionChangedProxy = WeakEvent.CollectionChanged.Add(originalObservableCollection, OriginalCollectionChanged);
            }
        }

        /// <summary>Override this method to free, release or reset any resources.</summary>
        /// <param name="disposing">if true then dispose unmanaged and managed resources; otherwise dispose only unmanaged resources.</param>
        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                originalCollectionChangedProxy?.Remove();
            }
            base.OnDispose(disposing);
        }
    }
}
