using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Waf.Foundation
{
    // Responsible to keep event handler alive throughout the lifetime of the target.
    internal sealed class WeakEventHandlerTable
    {
        private static readonly object staticPlaceholder = new();
        private readonly ConditionalWeakTable<object, object> weakTable = new();

        public void Add(Delegate targetHandler)
        {
            lock (weakTable)
            {
                var target = targetHandler.Target ?? staticPlaceholder;
                // Optimize for a single handler, if more handlers are added than use a list
                if (!weakTable.TryGetValue(target, out var value))
                {
                    weakTable.Add(target, targetHandler);
                }
                else
                {
                    if (value is not List<Delegate> list)
                    {
                        list = new List<Delegate> { (Delegate)value };
                        weakTable.Remove(target);
                        weakTable.Add(target, list);
                    }
                    list.Add(targetHandler);
                }
            }
        }

        public void Remove(Delegate targetHandler)
        {
            lock (weakTable)
            {
                var target = targetHandler.Target ?? staticPlaceholder;
                if (!weakTable.TryGetValue(target, out var value)) return;
                if (value is List<Delegate> list)
                {
                    var index = list.FindIndex(x => ReferenceEquals(x, targetHandler));
                    list.RemoveAt(index);
                    if (list.Count == 0) weakTable.Remove(target);
                }
                else
                {
                    if (!ReferenceEquals(value, targetHandler)) throw new InvalidOperationException("targetHandler instance to remove is not the one that was set.");
                    weakTable.Remove(target);
                }
            }
        }
    }
}
