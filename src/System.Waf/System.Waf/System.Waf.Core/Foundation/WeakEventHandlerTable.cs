using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Waf.Foundation
{
    // Responsible to keep event handler alive throughout the lifetime of the target.
    internal sealed class WeakEventHandlerTable
    {
        private static readonly object staticPlaceholder = new object();
        private readonly Lazy<ConditionalWeakTable<object, object>> weakTable;

        public WeakEventHandlerTable()
        {
            weakTable = new Lazy<ConditionalWeakTable<object, object>>(() => new ConditionalWeakTable<object, object>(), isThreadSafe: false);
        }

        public void Add(Delegate targetHandler)
        {
            lock (weakTable)
            {
                var table = weakTable.Value;
                var target = targetHandler.Target ?? staticPlaceholder;
                // Optimize for a single handler, if more handlers are added than use a list
                if (!table.TryGetValue(target, out var value))
                {
                    table.Add(target, targetHandler);
                }
                else
                {
                    if (!(value is List<Delegate> list))
                    {
                        list = new List<Delegate> { (Delegate)value };
                        table.Remove(target);
                        table.Add(target, list);
                    }
                    list.Add(targetHandler);
                }
            }
        }

        public void Remove(Delegate targetHandler)
        {
            lock (weakTable)
            {
                var table = weakTable.Value;
                var target = targetHandler.Target ?? staticPlaceholder;
                if (!table.TryGetValue(target, out var value)) return;
                if (value is List<Delegate> list)
                {
                    list.Remove(targetHandler);
                    if (list.Count == 0) table.Remove(target);
                }
                else
                {
                    table.Remove(target);
                }
            }
        }
    }
}
