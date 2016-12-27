using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Jbe.NewsReader.Domain.Foundation
{
    public class ObservableGroupedListView<TKey, TElement> : ObservableListViewBase<ObservableGroupingView<TKey, TElement>>, IDisposable
    {
        private readonly IEnumerable<TElement> originalList;
        private readonly Func<IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>> createGrouping;
        private readonly InnerListKeyComparer innerListKeyComparer;
        private readonly IEqualityComparer<TElement> elementComparer;
        private readonly INotifyCollectionChanged originalObservableCollection;
        private volatile bool isDisposed;


        public ObservableGroupedListView(IEnumerable<TElement> originalList, Func<IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>> createGrouping, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TElement> elementComparer)
            : base(new List<ObservableGroupingView<TKey, TElement>>())
        {
            this.originalList = originalList;
            this.createGrouping = createGrouping;
            innerListKeyComparer = new InnerListKeyComparer(keyComparer ?? EqualityComparer<TKey>.Default);
            this.elementComparer = elementComparer ?? EqualityComparer<TElement>.Default;

            InnerList.AddRange(CreateGroupingList());

            originalObservableCollection = originalList as INotifyCollectionChanged;
            if (originalObservableCollection != null)
            {
                originalObservableCollection.CollectionChanged += OriginalCollectionChanged;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        protected virtual void OnDispose(bool disposing) { }

        private IReadOnlyList<ObservableGroupingView<TKey, TElement>> CreateGroupingList()
        {
            return createGrouping(originalList).Select(x => new ObservableGroupingView<TKey, TElement>(x.Key, x)).ToArray();
        }

        private void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newGroupingList = CreateGroupingList();
            var intersect = InnerList.Intersect(newGroupingList, innerListKeyComparer);
            foreach (var target in intersect)
            {
                var source = newGroupingList.First(x => innerListKeyComparer.Equals(x, target));
                ListMerger.Merge(source, target, elementComparer, target.Insert, target.RemoveAt, () => target.Reset(source));
            }
            ListMerger.Merge(newGroupingList, InnerList, innerListKeyComparer, Insert, RemoveAt, () => Reset(newGroupingList));
        }


        private sealed class InnerListKeyComparer : IEqualityComparer<ObservableGroupingView<TKey, TElement>>
        {
            private readonly IEqualityComparer<TKey> keyComparer;

            public InnerListKeyComparer(IEqualityComparer<TKey> keyComparer)
            {
                this.keyComparer = keyComparer;
            }
            
            public bool Equals(ObservableGroupingView<TKey, TElement> x, ObservableGroupingView<TKey, TElement> y)
            {
                if (x != null && y != null) { return keyComparer.Equals(x.Key, y.Key); }
                if (x == null && y == null) { return true; }
                return false;
            }

            public int GetHashCode(ObservableGroupingView<TKey, TElement> obj)
            {
                return obj == null ? 0 : keyComparer.GetHashCode(obj.Key);
            }
        }
    }
}
