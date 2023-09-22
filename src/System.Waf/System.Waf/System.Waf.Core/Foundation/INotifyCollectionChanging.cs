using System.Collections.Specialized;

namespace System.Waf.Foundation
{
    /// <summary>Notifies listeners that the list is changing, such as when an item is added and removed or the whole list is cleared.</summary>
    public interface INotifyCollectionChanging
    {
        /// <summary>Occurs when the collection is changing. This event occurs before the change is applied to the collection.</summary>
        event NotifyCollectionChangedEventHandler? CollectionChanging;
    }
}
