using System.Collections.Generic;
using System.Linq;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Domain.Foundation
{
    public class ObservableGroupingView<TKey, TElement> : ObservableListViewBase<TElement>, IGrouping<TKey, TElement>
    {
        public ObservableGroupingView(TKey key, IEnumerable<TElement> originalList) 
            : base(originalList)
        {
            Key = key;
        }

        public TKey Key { get; }

        internal new List<TElement> InnerList => base.InnerList;


        internal new void Insert(int newItemIndex, TElement newItem)
        {
            base.Insert(newItemIndex, newItem);
        }

        internal new void RemoveAt(int oldItemIndex)
        {
            base.RemoveAt(oldItemIndex);
        }

        internal new void Reset(IReadOnlyList<TElement> newList)
        {
            base.Reset(newList);
        }
    }
}
