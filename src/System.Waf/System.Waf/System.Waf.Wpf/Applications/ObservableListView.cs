﻿using System.Collections.Generic;
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
    [Obsolete("Use System.Waf.Foundation.ObservableListViewCore instead.")]
    public class ObservableListView<T> : ObservableListViewCore<T>
    {
        /// <summary>Initializes a new instance of the ObservableListView class that represents a view of the specified list.</summary>
        /// <param name="originalList">The original list.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        public ObservableListView(IEnumerable<T> originalList) : this(originalList, null, null, null)
        {
        }

        /// <summary>Initializes a new instance of the ObservableListView class that represents a view of the specified list.</summary>
        /// <param name="originalList">The original list.</param>
        /// <param name="comparer">Optional, a custom comparer used to compare the items.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        public ObservableListView(IEnumerable<T> originalList, IEqualityComparer<T>? comparer) : this(originalList, comparer, null, null)
        {
        }

        /// <summary>Initializes a new instance of the ObservableListView class that represents a view of the specified list.</summary>
        /// <param name="originalList">The original list.</param>
        /// <param name="comparer">Optional, a custom comparer used to compare the items.</param>
        /// <param name="filter">Optional, a filter used for this list view.</param>
        /// <param name="sort">Optional, a sorting used for this list view.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        public ObservableListView(IEnumerable<T> originalList, IEqualityComparer<T>? comparer, Predicate<T>? filter,
            Func<IEnumerable<T>, IOrderedEnumerable<T>>? sort) : base(originalList, comparer, filter, sort, false)
        {
        }
    }
}
