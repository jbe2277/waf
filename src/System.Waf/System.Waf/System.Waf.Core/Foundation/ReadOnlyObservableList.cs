using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Waf.Foundation
{
    /// <summary>
    /// Represents a read-only <see cref="ObservableCollection{T}"/>.
    /// This class implements the IReadOnlyObservableList interface and provides public CollectionChanged and PropertyChanged events.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ReadOnlyObservableList<T> : ReadOnlyObservableCollection<T>, INotifyCollectionChanging, IReadOnlyObservableList<T>
    {
        private readonly bool collectionChangingSupported;
        [NonSerialized] private NotifyCollectionChangedEventHandler? collectionChanging;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyObservableCollection{T}"/>
        /// class that serves as a wrapper around the specified <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <param name="list">The <see cref="ObservableCollection{T}"/> with which to create this instance of the 
        /// <see cref="ReadOnlyObservableCollection{T}"/> class.</param>
        /// <exception cref="ArgumentNullException">list is null.</exception>
        public ReadOnlyObservableList(ObservableCollection<T> list) : base(list)
        {
            if (list is INotifyCollectionChanging x) { collectionChangingSupported = true; x.CollectionChanging += HandleCollectionChanging; }
        }

        /// <summary>Gets an empty <see cref="ReadOnlyObservableList{T}"/>.</summary>
        /// <remarks>The returned instance is immutable and will always be empty.</remarks>
        public static ReadOnlyObservableList<T> Empty { get; } = new(new ObservableList<T>());

        private void CheckCollectionChanging() { if (!collectionChangingSupported) throw new NotSupportedException("CollectionChanging event is not supported by the underlying list."); }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">CollectionChanging event is not supported by the underlying list.</exception>
        public event NotifyCollectionChangedEventHandler? CollectionChanging
        {
            add { CheckCollectionChanging(); collectionChanging += value; }
            remove { collectionChanging -= value; }
        }

        /// <inheritdoc />
        public new event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => base.CollectionChanged += value;
            remove => base.CollectionChanged -= value;
        }

        /// <inheritdoc />
        public new event PropertyChangedEventHandler? PropertyChanged
        {
            add => base.PropertyChanged += value;
            remove => base.PropertyChanged -= value;
        }

        /// <summary>Raises the CollectionChanged event with the provided arguments.</summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e) => collectionChanging?.Invoke(this, e);

        private void HandleCollectionChanging(object? sender, NotifyCollectionChangedEventArgs e) => OnCollectionChanging(e);        
    }
}
