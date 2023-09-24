using System.ComponentModel;

namespace System.Waf.Foundation
{
    /// <summary>Notifies listeners that a property of a collection item changed.</summary>
    public interface INotifyCollectionItemChanged
    {
        /// <summary>Occurs when a collection item property changed. The sender argument of this event is the item and not the collection.</summary>
        event PropertyChangedEventHandler? CollectionItemChanged;
    }
}
