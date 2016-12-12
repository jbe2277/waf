using System.Collections.Generic;
using System.Linq;

namespace Jbe.NewsReader.Domain.Foundation
{
    public class ObservableGroupingView<TKey, TElement> : ObservableListView<TElement>, IGrouping<TKey, TElement>
    {
        public ObservableGroupingView(TKey key, IEnumerable<TElement> originalList, IEqualityComparer<TElement> elementComparer) 
            : base(originalList, elementComparer)
        {
            Key = key;
        }

        public TKey Key { get; }
    }
}
