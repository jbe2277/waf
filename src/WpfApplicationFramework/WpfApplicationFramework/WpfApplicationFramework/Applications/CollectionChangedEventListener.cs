using System.Collections.Specialized;
using System.Windows;

namespace System.Waf.Applications
{
    internal class CollectionChangedEventListener : IWeakEventListener
    {
        public CollectionChangedEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (handler == null) { throw new ArgumentNullException(nameof(handler)); }
            Source = source;
            Handler = handler;
        }


        public INotifyCollectionChanged Source { get; }

        public NotifyCollectionChangedEventHandler Handler { get; }


        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            Handler(sender, (NotifyCollectionChangedEventArgs)e);
            return true;
        }
    }
}
