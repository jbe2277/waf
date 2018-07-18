using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace System.Waf.Foundation
{
    /// <summary>
    /// Represents a observable list view for filtering and sorting a data collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ObservableListView<T> : ObservableListViewBase<T>, IDisposable
    {
        private readonly IEnumerable<T> originalList;
        private readonly IEqualityComparer<T> comparer;
        private readonly INotifyCollectionChanged originalObservableCollection;
        private Predicate<T> filter;
        private Func<IEnumerable<T>, IOrderedEnumerable<T>> sort;
        private volatile bool isDisposed;


        /// <summary>
        /// Initializes a new instance of the ObservableListView class that represents a view of the specified list.
        /// </summary>
        /// <param name="originalList">The orignal list.</param>
        public ObservableListView(IEnumerable<T> originalList) : this(originalList, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ObservableListView class that represents a view of the specified list.
        /// </summary>
        /// <param name="originalList">The orignal list.</param>
        /// <param name="comparer">The IEqualityComparer implementation to use when comparing items.</param>
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


        /// <summary>
        /// Gets or sets the filter used for this list view.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the sorting used for this list view.
        /// </summary>
        public Func<IEnumerable<T>, IOrderedEnumerable<T>> Sort
        {
            get { return sort; }
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
        /// If the original list implements the INotifyCollectionChanged interface then calling this method is not necessary.
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
