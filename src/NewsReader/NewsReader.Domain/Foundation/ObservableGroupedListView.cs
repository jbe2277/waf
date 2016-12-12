using System;
using System.Collections.Generic;
using System.Linq;

namespace Jbe.NewsReader.Domain.Foundation
{
    public class ObservableGroupedListView<TKey, TElement> : ObservableListView<ObservableGroupingView<TKey, TElement>>
    {
        private readonly IEnumerable<TElement> originalList;
        private readonly Func<IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>> createGrouping;
        private readonly IEqualityComparer<TKey> keyComparer;
        private readonly IEqualityComparer<TElement> elementComparer;
        private readonly List<ObservableGroupingView<TKey, TElement>> innerList;


        public ObservableGroupedListView(IEnumerable<TElement> originalList, Func<IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>> createGrouping, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TElement> elementComparer)
            : base(new List<ObservableGroupingView<TKey, TElement>>()) 
        {
            this.originalList = originalList;
            this.createGrouping = createGrouping;
            this.keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
            this.elementComparer = elementComparer ?? EqualityComparer<TElement>.Default;

            innerList = (List<ObservableGroupingView<TKey, TElement>>)Items;
            innerList.AddRange(createGrouping(originalList).Select(x => new ObservableGroupingView<TKey, TElement>(x.Key, x, elementComparer)));
        }
    }
}
