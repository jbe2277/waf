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
    public class ReadOnlyObservableList<T> : ReadOnlyObservableCollection<T>, IReadOnlyObservableList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyObservableCollection{T}"/>
        /// class that serves as a wrapper around the specified <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <param name="list">
        /// The <see cref="ObservableCollection{T}"/> with which to create this instance of the <see cref="ReadOnlyObservableCollection{T}"/> class.
        /// </param>
        /// <exception cref="ArgumentNullException">list is null.</exception>
        public ReadOnlyObservableList(ObservableCollection<T> list)
            : base(list)
        {
        }


        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public new event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => base.CollectionChanged += value;
            remove => base.CollectionChanged -= value;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public new event PropertyChangedEventHandler PropertyChanged
        {
            add => base.PropertyChanged += value;
            remove => base.PropertyChanged -= value;
        }
    }
}
