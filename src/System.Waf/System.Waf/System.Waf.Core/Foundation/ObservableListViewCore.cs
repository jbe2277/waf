using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace System.Waf.Foundation
{
    /// <summary>
    /// Represents a observable list view for filtering and sorting a data collection.
    /// When the original collection notifies a change via the <see cref="INotifyCollectionChanged"/> interface then
    /// this view updates automatically.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ObservableListViewCore<T> : ObservableListViewBase<T>, IDisposable
    {
        private readonly IEnumerable<T> originalList;
        private readonly IEqualityComparer<T> comparer;
        private readonly INotifyCollectionChanged originalObservableCollection;
        private readonly bool noCollectionChangedHandler;
        private Predicate<T> filter;
        private Func<IEnumerable<T>, IOrderedEnumerable<T>> sort;
        private volatile bool isDisposed;


        /// <summary>
        /// Initializes a new instance of the ObservableListView class that represents a view of the specified list.
        /// </summary>
        /// <param name="originalList">The orignal list.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        public ObservableListViewCore(IEnumerable<T> originalList) : this(originalList, null, null, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ObservableListView class that represents a view of the specified list.
        /// </summary>
        /// <param name="originalList">The orignal list.</param>
        /// <param name="comparer">Optional, a custom comparer used to compare the items.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        public ObservableListViewCore(IEnumerable<T> originalList, IEqualityComparer<T> comparer) : this(originalList, comparer, null, null, false)
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
        public ObservableListViewCore(IEnumerable<T> originalList, IEqualityComparer<T> comparer, Predicate<T> filter,
            Func<IEnumerable<T>, IOrderedEnumerable<T>> sort) : this(originalList, comparer, filter, sort, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ObservableListView class that represents a view of the specified list.
        /// </summary>
        /// <param name="originalList">The orignal list.</param>
        /// <param name="comparer">Optional, a custom comparer used to compare the items.</param>
        /// <param name="filter">Optional, a filter used for this list view.</param>
        /// <param name="sort">Optional, a sorting used for this list view.</param>
        /// <param name="noCollectionChangedHandler">Pass true when the subclass takes care about the collection changed event of the originalCollection. Default is false.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        public ObservableListViewCore(IEnumerable<T> originalList, IEqualityComparer<T> comparer, Predicate<T> filter, 
            Func<IEnumerable<T>, IOrderedEnumerable<T>> sort, bool noCollectionChangedHandler) : base(originalList)
        {
            this.originalList = originalList ?? throw new ArgumentNullException(nameof(originalList));
            this.comparer = comparer ?? EqualityComparer<T>.Default;
            this.filter = filter;
            this.sort = sort;
            this.noCollectionChangedHandler = noCollectionChangedHandler;
            if (this.filter != null || this.sort != null) UpdateInnerList();

            if (!noCollectionChangedHandler)
            {
                originalObservableCollection = originalList as INotifyCollectionChanged;
                if (originalObservableCollection != null)
                {
                    originalObservableCollection.CollectionChanged += OriginalCollectionChanged;
                }
            }
        }


        /// <summary>
        /// Gets or sets the filter used for this list view.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the sorting used for this list view.
        /// </summary>
        public Func<IEnumerable<T>, IOrderedEnumerable<T>> Sort
        {
            get => sort;
            set
            {
                if (sort != value)
                {
                    sort = value;
                    UpdateInnerList();
                }
            }
        }


        /// <summary>
        /// Updates the list view and raises the appropriate collection changed events.
        /// </summary>
        public void Update()
        {
            UpdateInnerList();
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
                if (!noCollectionChangedHandler && originalObservableCollection != null)
                {
                    originalObservableCollection.CollectionChanged -= OriginalCollectionChanged;
                }
            }
        }

        /// <summary>
        /// Override this method to free, release or reset any resources.
        /// </summary>
        /// <param name="disposing">if true then dispose unmanaged and managed resources; otherwise dispose only unmanaged resources.</param>
        protected virtual void OnDispose(bool disposing) { }

        private void UpdateInnerList()
        {
            var enumerable = originalList;
            if (Filter != null)
            {
                enumerable = enumerable.Where(x => Filter(x));
            }
            if (Sort != null)
            {
                enumerable = Sort(enumerable);
            }
            var newList = enumerable.ToArray();
            InnerList.Merge(newList.ToArray(), comparer, Insert, RemoveAt, () => Reset(newList), Move);
        }

        private void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateInnerList();
        }
    }
}
