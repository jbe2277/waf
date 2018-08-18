using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Waf.Foundation;

namespace System.Waf.Applications
{
    /// <summary>
    /// Represents a observable list view for filtering and sorting a data collection.
    /// When the original collection notifies a change via the <see cref="INotifyCollectionChanged"/> interface then
    /// this view updates automatically.
    /// </summary>
    /// <remarks>
    /// In comparison to the base class this one uses weak event handlers to listen for collection changed events.
    /// Therefore, it does not produce a memory leak if Dispose is not called.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ObservableListView<T> : ObservableListViewCore<T>
    {
        private readonly INotifyCollectionChanged originalObservableCollection;


        /// <summary>
        /// Initializes a new instance of the ObservableListView class that represents a view of the specified list.
        /// </summary>
        /// <param name="originalList">The orignal list.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        public ObservableListView(IEnumerable<T> originalList) : this(originalList, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ObservableListView class that represents a view of the specified list.
        /// </summary>
        /// <param name="originalList">The orignal list.</param>
        /// <param name="comparer">Optional, a custom comparer used to compare the items.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        public ObservableListView(IEnumerable<T> originalList, IEqualityComparer<T> comparer) : this(originalList, comparer, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ObservableListView class that represents a view of the specified list.
        /// </summary>
        /// <param name="originalList">The orignal list.</param>
        /// <param name="comparer">Optional, a custom comparer used to compare the items.</param>
        /// <param name="filter">Optional, a filter used for this list view.</param>
        /// <param name="sort">Optional, a sorting used for this list view.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        public ObservableListView(IEnumerable<T> originalList, IEqualityComparer<T> comparer, Predicate<T> filter,
            Func<IEnumerable<T>, IOrderedEnumerable<T>> sort) : base(originalList, comparer, filter, sort, true)
        {
            originalObservableCollection = originalList as INotifyCollectionChanged;
            if (originalObservableCollection != null)
            {
                CollectionChangedEventManager.AddHandler(originalObservableCollection, OriginalCollectionChanged);
            }
        }

        /// <summary>
        /// Override this method to free, release or reset any resources.
        /// </summary>
        /// <param name="disposing">if true then dispose unmanaged and managed resources; otherwise dispose only unmanaged resources.</param>
        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (originalObservableCollection != null)
                {
                    CollectionChangedEventManager.RemoveHandler(originalObservableCollection, OriginalCollectionChanged);
                }
            }
            base.OnDispose(disposing);
        }

        private void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Update();
        }
    }
}
