using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Waf.Foundation
{
    /// <summary>Represents a dynamic data collection that provides notifications when items get added or removed, or when the whole list is refreshed.
    /// It extends the <see cref="ObservableCollection{T}"/> by implementing the <see cref="INotifyCollectionChanging"/> and the <see cref="INotifyCollectionItemChanged"/> interface.</summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ObservableList<T> : ObservableCollection<T>, IReadOnlyObservableList<T>
    {
        private readonly Dictionary<INotifyPropertyChanged, (int count, IWeakEventProxy proxy)> weakEventProxies = new();

        /// <summary>Initializes a new instance of the <see cref="ObservableCollection{T}"/> class.</summary>
        public ObservableList() { }

        /// <summary>Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that contains elements copied from the specified collection.</summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <exception cref="ArgumentNullException">The collection parameter cannot be null.</exception>
        public ObservableList(IEnumerable<T> collection) : base(collection)
        {
            foreach (var x in this) TryAddItemPropertyChanged(x);
        }

        /// <summary>Occurs when a property value changes.</summary>
        public new event PropertyChangedEventHandler? PropertyChanged
        {
            add => base.PropertyChanged += value;
            remove => base.PropertyChanged -= value;
        }

        /// <inheritdoc />
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanging;

        /// <inheritdoc />
        [field: NonSerialized] public event PropertyChangedEventHandler? CollectionItemChanged;

        /// <inheritdoc />
        protected override void ClearItems()
        {
            ClearItemPropertyChanged();
            OnCollectionChanging(EventArgsCache.ResetCollectionChanged);
            base.ClearItems();
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            T removedItem = this[index];
            TryRemoveItemPropertyChanged(removedItem);
            OnCollectionChanging(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, T item)
        {
            OnCollectionChanging(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            base.InsertItem(index, item);
            TryAddItemPropertyChanged(item);
        }

        /// <inheritdoc />
        protected override void SetItem(int index, T item)
        {
            T originalItem = this[index];
            TryRemoveItemPropertyChanged(originalItem);
            OnCollectionChanging(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, originalItem, index));
            base.SetItem(index, item);
            TryAddItemPropertyChanged(item);
        }

        /// <inheritdoc />
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            T movedItem = this[oldIndex];
            OnCollectionChanging(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, movedItem, newIndex, oldIndex));
            base.MoveItem(oldIndex, newIndex);
        }

        /// <summary>Raises the CollectionChanged event with the provided arguments.</summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e) => CollectionChanging?.Invoke(this, e);

        /// <summary>Raise the CollectionItemChanged event with the provided arguments.</summary>
        /// <param name="item">The collection item that raised the PropertyChanged event.</param>
        /// <param name="e">The PropertyChanged event argument.</param>
        protected virtual void OnCollectionItemChanged(object? item, PropertyChangedEventArgs e) => CollectionItemChanged?.Invoke(item, e);

        private void ItemPropertyChanged(object? sender, PropertyChangedEventArgs e) => OnCollectionItemChanged(sender, e);

        private void TryAddItemPropertyChanged(T? item)
        {
            if (item is not INotifyPropertyChanged observable) return;
            if (weakEventProxies.TryGetValue(observable, out var x))
            {
                weakEventProxies[observable] = (x.count + 1, x.proxy);
            }
            else
            {
                weakEventProxies.Add(observable, (1, WeakEvent.PropertyChanged.Add(observable, ItemPropertyChanged)));
            }
        }

        private void TryRemoveItemPropertyChanged(T? item)
        {
            if (item is not INotifyPropertyChanged observable) return;
            if (weakEventProxies.TryGetValue(observable, out var x))
            {
                if (x.count <= 1)
                {
                    x.proxy.Remove();
                    weakEventProxies.Remove(observable);
                }
                else weakEventProxies[observable] = (x.count - 1, x.proxy);
            }
        }

        private void ClearItemPropertyChanged()
        {
            foreach (var x in weakEventProxies.Values) x.proxy.Remove();
            weakEventProxies.Clear();
        }
    }
}
