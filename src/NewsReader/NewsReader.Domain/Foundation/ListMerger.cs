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
                    for (int i = 0, j = 0; i < target.Count; i++, j++)
                    {
                        if (!comparer.Equals(target[i], source[j]))
                        {
                            if (newItemIndex != -1)
                            {
                                // Second change is not supported -> Reset
                                resetAction();
                                return;
                            }
                            newItemIndex = j;
                            i--;
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
                    for (int i = 0, j = 0; j < source.Count; i++, j++)
                    {
                        if (!comparer.Equals(target[i], source[j]))
                        {
                            if (oldItemIndex != -1)
                            {
                                // Second change is not supported -> Reset
                                resetAction();
                                return;
                            }
                            oldItemIndex = i;
                            j--;
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
