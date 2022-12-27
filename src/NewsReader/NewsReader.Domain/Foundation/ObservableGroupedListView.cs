using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Waf.Foundation;

namespace Waf.NewsReader.Domain.Foundation;

public class ObservableGroupedListView<TKey, TElement> : ObservableListViewBase<ObservableGroupingView<TKey, TElement>>, IDisposable
{
    private readonly IEnumerable<TElement> originalList;
    private readonly Func<IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>> createGrouping;
    private readonly InnerListKeyComparer innerListKeyComparer;
    private readonly IEqualityComparer<TElement> elementComparer;
    private readonly INotifyCollectionChanged? originalObservableCollection;
    private Predicate<TElement>? filter;
    private volatile bool isDisposed;

    public ObservableGroupedListView(IEnumerable<TElement> originalList, Func<IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>> createGrouping)
        : this(originalList, createGrouping, null, null) { }

    public ObservableGroupedListView(IEnumerable<TElement> originalList, Func<IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>> createGrouping,
        IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TElement>? elementComparer)
        : base(new List<ObservableGroupingView<TKey, TElement>>())
    {
        this.originalList = originalList;
        this.createGrouping = createGrouping;
        innerListKeyComparer = new InnerListKeyComparer(keyComparer ?? EqualityComparer<TKey>.Default);
        this.elementComparer = elementComparer ?? EqualityComparer<TElement>.Default;
        InnerList.AddRange(CreateGroupingList());
        originalObservableCollection = originalList as INotifyCollectionChanged;
        if (originalObservableCollection != null) originalObservableCollection.CollectionChanged += OriginalCollectionChanged;
    }

    public Predicate<TElement>? Filter
    {
        get => filter;
        set
        {
            if (filter == value) return;
            filter = value;
            UpdateInnerList();
        }
    }

    public void Refresh() => UpdateInnerList();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (isDisposed) return;
        isDisposed = true;
        OnDispose(disposing);
        if (disposing)
        {
            if (originalObservableCollection != null) originalObservableCollection.CollectionChanged -= OriginalCollectionChanged;
        }
    }

    protected virtual void OnDispose(bool disposing) { }

    private void UpdateInnerList()
    {
        var newGroupingList = CreateGroupingList();
        var intersect = InnerList.Intersect(newGroupingList, innerListKeyComparer);
        foreach (var target in intersect)
        {
            var source = newGroupingList.First(x => innerListKeyComparer.Equals(x, target));
            CollectionHelper.Merge(target, source, elementComparer, target.Insert, target.RemoveAt, () => target.Reset(source));
        }
        CollectionHelper.Merge(InnerList, newGroupingList, innerListKeyComparer, Insert, RemoveAt, () => Reset(newGroupingList));
    }

    private IReadOnlyList<ObservableGroupingView<TKey, TElement>> CreateGroupingList()
    {
        IEnumerable<TElement> filteredList = originalList;
        if (Filter != null) filteredList = originalList.Where(x => Filter(x)).ToArray();
        return createGrouping(filteredList).Select(x => new ObservableGroupingView<TKey, TElement>(x.Key, x)).ToArray();
    }

    private void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => UpdateInnerList();


    private sealed class InnerListKeyComparer : IEqualityComparer<ObservableGroupingView<TKey, TElement>>
    {
        private readonly IEqualityComparer<TKey> keyComparer;

        public InnerListKeyComparer(IEqualityComparer<TKey> keyComparer)
        {
            this.keyComparer = keyComparer;
        }

        public bool Equals(ObservableGroupingView<TKey, TElement> x, ObservableGroupingView<TKey, TElement> y)
        {
            if (x != null && y != null) return keyComparer.Equals(x.Key, y.Key);
            if (x == null && y == null) return true;
            return false;
        }

        public int GetHashCode(ObservableGroupingView<TKey, TElement> obj) => obj == null ? 0 : keyComparer.GetHashCode(obj.Key);
    }
}
