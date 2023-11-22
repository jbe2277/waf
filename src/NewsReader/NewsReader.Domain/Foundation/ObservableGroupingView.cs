namespace Waf.NewsReader.Domain.Foundation;

public class ObservableGroupingView<TKey, TElement>(TKey key, IEnumerable<TElement> originalList) : ObservableListViewBase<TElement>(originalList), IGrouping<TKey, TElement>
{
    public TKey Key { get; } = key;

    internal new List<TElement> InnerList => base.InnerList;

    internal new void Insert(int newItemIndex, TElement newItem) => base.Insert(newItemIndex, newItem);

    internal new void RemoveAt(int oldItemIndex) => base.RemoveAt(oldItemIndex);

    internal new void Reset(IEnumerable<TElement> newList) => base.Reset(newList);
}
