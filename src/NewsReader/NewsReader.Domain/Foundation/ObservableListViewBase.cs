using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Domain.Foundation
{
    public abstract class ObservableListViewBase<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyObservableList<T>
    {
        private const string indexerName = "Item[]";  // This must be equal to Binding.IndexerName
        private static readonly PropertyChangedEventArgs CountChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
        private static readonly PropertyChangedEventArgs IndexerChangedEventArgs = new PropertyChangedEventArgs(indexerName);

        private readonly IEnumerable<T> originalList;
        

        protected ObservableListViewBase(IEnumerable<T> originalList) : base(new List<T>())
        {
            this.originalList = originalList;

            InnerList = (List<T>)Items;
            InnerList.AddRange(originalList);
        }


        protected List<T> InnerList { get; }


        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;
        
        
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected void Insert(int newItemIndex, T newItem)
        {
            InnerList.Insert(newItemIndex, newItem);
            OnPropertyChanged(CountChangedEventArgs);
            OnPropertyChanged(IndexerChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, newItemIndex));
        }

        protected void RemoveAt(int oldItemIndex)
        {
            var oldItem = InnerList[oldItemIndex];
            InnerList.RemoveAt(oldItemIndex);
            OnPropertyChanged(CountChangedEventArgs);
            OnPropertyChanged(IndexerChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, oldItemIndex));
        }

        protected void Reset(IReadOnlyList<T> newList)
        {
            InnerList.Clear();
            InnerList.AddRange(newList);
            OnPropertyChanged(CountChangedEventArgs);
            OnPropertyChanged(IndexerChangedEventArgs);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
