using System;
using System.Collections.Generic;
using System.Linq;

namespace Jbe.NewsReader.Domain.Foundation
{
    public static class ListMerger
    {
        public static void Merge<T>(IList<T> source, IList<T> target, IEqualityComparer<T> comparer = null,
            Action<int, T> insertAction = null, Action<int> removeAtAction = null, Action resetAction = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            insertAction = insertAction ?? target.Insert;
            removeAtAction = removeAtAction ?? target.RemoveAt;
            resetAction = resetAction ?? (() => 
            {
                foreach (var item in target.ToArray()) { target.Remove(item); }  // Avoid Clear because of CollectionChanged events
                foreach (var item in source) { target.Add(item); }
            });
            
            if (target.SequenceEqual(source, comparer))
            {
                return;
            }

            // Item added or removed
            if (target.Count != source.Count)
            {
                // Change of more than 1 item added or removed is not supported -> Reset
                if (Math.Abs(target.Count - source.Count) != 1)
                {
                    resetAction();
                    return;
                }

                if (target.Count < source.Count)
                {
                    int newItemIndex = -1;
                    for (int t = 0, s = 0; t < target.Count; t++, s++)
                    {
                        if (!comparer.Equals(target[t], source[s]))
                        {
                            if (newItemIndex != -1)
                            {
                                // Second change is not supported -> Reset
                                resetAction();
                                return;
                            }
                            newItemIndex = s;
                            t--;
                        }
                    }
                    if (newItemIndex == -1)
                    {
                        newItemIndex = source.Count - 1;
                    }
                    insertAction(newItemIndex, source[newItemIndex]);
                    return;
                }
                else
                {
                    int oldItemIndex = -1;
                    for (int t = 0, s = 0; s < source.Count; t++, s++)
                    {
                        if (!comparer.Equals(target[t], source[s]))
                        {
                            if (oldItemIndex != -1)
                            {
                                // Second change is not supported -> Reset
                                resetAction();
                                return;
                            }
                            oldItemIndex = t;
                            s--;
                        }
                    }
                    if (oldItemIndex == -1)
                    {
                        oldItemIndex = target.Count - 1;
                    }
                    removeAtAction(oldItemIndex);
                    return;
                }
            }

            resetAction();
        }
    }
}
