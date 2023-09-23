﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Waf.Foundation
{
    /// <summary>Represents a dynamic data collection that provides notifications when items get added or removed, or when the whole list is refreshed.
    /// It extends the <see cref="ObservableCollection{T}"/> by implementing the <see cref="INotifyCollectionChanging"/> interface.</summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ObservableList<T> : ObservableCollection<T>, INotifyCollectionChanging, IReadOnlyObservableList<T>
    {
        /// <summary>Initializes a new instance of the <see cref="ObservableCollection{T}"/> class.</summary>
        public ObservableList() { }

        /// <summary>Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that contains elements copied from the specified collection.</summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <exception cref="ArgumentNullException">The collection parameter cannot be null.</exception>
        public ObservableList(IEnumerable<T> collection) : base(collection) { }

        /// <summary>Occurs when a property value changes.</summary>
        public new event PropertyChangedEventHandler? PropertyChanged
        {
            add => base.PropertyChanged += value;
            remove => base.PropertyChanged -= value;
        }

        /// <inheritdoc />
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanging;

        /// <inheritdoc />
        protected override void ClearItems()
        {
            OnCollectionChanging(EventArgsCache.ResetCollectionChanging);
            base.ClearItems();
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            T removedItem = this[index];
            OnCollectionChanging(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, T item)
        {
            OnCollectionChanging(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        protected override void SetItem(int index, T item)
        {
            T originalItem = this[index];
            OnCollectionChanging(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, originalItem, index));
            base.SetItem(index, item);
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
    }

    internal static class EventArgsCache
    {
        internal static readonly NotifyCollectionChangedEventArgs ResetCollectionChanging = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    }
}