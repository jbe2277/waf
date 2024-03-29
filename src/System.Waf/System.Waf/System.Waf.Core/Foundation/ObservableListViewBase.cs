﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace System.Waf.Foundation
{
    /// <summary>Provides the base class for a generic observable read-only collection.</summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public abstract class ObservableListViewBase<T> : ReadOnlyCollection<T>, IReadOnlyObservableList<T>
    {
        private readonly Dictionary<object, IWeakEventProxy> weakEventProxies = new();
        private int deferredChanges;
        private int deferCount;

        /// <summary>Initializes a new instance of the ObservableListViewBase class.</summary>
        /// <param name="originalList">Initialize the list view with the items from this list.</param>
        protected ObservableListViewBase(IEnumerable<T>? originalList) : base(new List<T>())
        {
            InnerList = (List<T>)Items;
            if (originalList != null)
            {
                InnerList.AddRange(originalList);
                foreach (var x in InnerList) TryAddItemPropertyChanged(x);
            }
        }

        /// <summary>The inner list of this list view.</summary>
        protected List<T> InnerList { get; }

        /// <inheritdoc />
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanging;

        /// <inheritdoc />
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <inheritdoc />
        [field: NonSerialized] public event PropertyChangedEventHandler? CollectionItemChanged;

        /// <inheritdoc />
        [field: NonSerialized] public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>Defer collection changed notifications until Dispose is called on the returned object. If the collection was changed then a CollectionChanged Reset event will be raised.</summary>
        /// <returns>Object used to stop the deferral.</returns>
        public IDisposable DeferCollectionChangedNotifications()
        {
            Interlocked.Increment(ref deferCount);
            return new DisposedNotifier(() =>
            {
                if (Interlocked.Decrement(ref deferCount) == 0 && Interlocked.Exchange(ref deferredChanges, 0) > 0)
                {
                    OnCollectionChanged(EventArgsCache.ResetCollectionChanged);
                }
            });
        }

        /// <summary>Raises the CollectionChanged event with the provided arguments.</summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            if (deferCount <= 0) CollectionChanging?.Invoke(this, e);
            else if (deferredChanges == 0) CollectionChanging?.Invoke(this, EventArgsCache.ResetCollectionChanged);
        }

        /// <summary>Raises the CollectionChanged event with the provided arguments.</summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (deferCount <= 0) CollectionChanged?.Invoke(this, e);
            else Interlocked.Increment(ref deferredChanges);
        }

        /// <summary>Raise the CollectionItemChanged event with the provided arguments.</summary>
        /// <param name="item">The collection item that raised the PropertyChanged event.</param>
        /// <param name="e">The PropertyChanged event argument.</param>
        protected virtual void OnCollectionItemChanged(object? item, PropertyChangedEventArgs e) => CollectionItemChanged?.Invoke(item, e);

        /// <summary>Raises the PropertyChanged event with the provided arguments.</summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        /// <summary>Inserts an element into the list at the specified index.</summary>
        /// <param name="newItemIndex">The zero-based index at which item should be inserted.</param>
        /// <param name="newItem">The object to insert.</param>
        protected void Insert(int newItemIndex, [AllowNull] T newItem)
        {
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, newItemIndex);
            OnCollectionChanging(e);
            InnerList.Insert(newItemIndex, newItem!);
            TryAddItemPropertyChanged(newItem);
            OnPropertyChanged(EventArgsCache.CountPropertyChanged);
            OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
            OnCollectionChanged(e);
        }

        /// <summary>Removes the element at the specified index.</summary>
        /// <param name="oldItemIndex">The zero-based index of the element to remove.</param>
        protected void RemoveAt(int oldItemIndex)
        {
            var oldItem = InnerList[oldItemIndex];
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, oldItemIndex);
            OnCollectionChanging(e);
            InnerList.RemoveAt(oldItemIndex);
            TryRemoveItemPropertyChanged(oldItem);
            OnPropertyChanged(EventArgsCache.CountPropertyChanged);
            OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
            OnCollectionChanged(e);
        }

        /// <summary>Replace all items with the items of the new list.</summary>
        /// <param name="newList">The items of the new list.</param>
        protected void Reset(IEnumerable<T> newList)
        {
            OnCollectionChanging(EventArgsCache.ResetCollectionChanged);
            foreach (var x in InnerList) TryRemoveItemPropertyChanged(x);
            InnerList.Clear();
            InnerList.AddRange(newList);
            foreach (var x in InnerList) TryAddItemPropertyChanged(x);
            OnPropertyChanged(EventArgsCache.CountPropertyChanged);
            OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
            OnCollectionChanged(EventArgsCache.ResetCollectionChanged);
        }

        /// <summary>Moves the item at the specified index to a new location in the collection.</summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        protected void Move(int oldIndex, int newIndex)
        {
            T item = InnerList[oldIndex];
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
            OnCollectionChanging(e);
            InnerList.RemoveAt(oldIndex);
            InnerList.Insert(newIndex, item);
            OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
            OnCollectionChanged(e);
        }

        private void ItemPropertyChanged(object? sender, PropertyChangedEventArgs e) => OnCollectionItemChanged(sender, e);

        private void TryAddItemPropertyChanged(T? item) 
        { 
            if (item is INotifyPropertyChanged x) weakEventProxies.Add(x, WeakEvent.PropertyChanged.Add(x, ItemPropertyChanged)); 
        }

        private void TryRemoveItemPropertyChanged(T? item) 
        {
            if (item is null) return;
            if (weakEventProxies.TryGetValue(item, out var proxy))
            {
                proxy.Remove();
                weakEventProxies.Remove(item);
            }
        }

        private sealed class DisposedNotifier : IDisposable
        {
            private Action? disposed;

            public DisposedNotifier(Action disposed)
            {
                this.disposed = disposed;
            }

            public void Dispose() => Interlocked.Exchange(ref disposed, null)?.Invoke();
        }
    }
}
