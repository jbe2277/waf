using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Waf.Foundation
{
    /// <summary>Provides helper methods for collections.</summary>
    public static class CollectionHelper
    {
        /// <summary>Searches for the specified object and returns the zero-based index of the first occurrence within the entire collection.</summary>
        /// <typeparam name="T">The collection item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The object to locate in the collection. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire collection, if found; otherwise, –1.</returns>
        /// <exception cref="ArgumentNullException">collection must not be <c>null</c>.</exception>
        public static int IndexOf<T>(this IEnumerable<T> collection, [AllowNull] T item)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (collection is IList<T> list) return list.IndexOf(item);

            int i = 0;
            foreach (T localItem in collection)
            {
                if (Equals(localItem, item)) return i;
                i++;
            }
            return -1;
        }

        /// <summary>Gets the next element in the collection or default when no next element can be found.</summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="current">The current item.</param>
        /// <returns>The next element in the collection or default when no next element can be found.</returns>
        /// <exception cref="ArgumentNullException">collection must not be <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The collection does not contain the specified current item.</exception>
        public static T GetNextElementOrDefault<T>(this IEnumerable<T> collection, [AllowNull] T current)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            using var enumerator = collection.GetEnumerator();
            bool found = false;
            while (enumerator.MoveNext())
            {
                if (EqualityComparer<T>.Default.Equals(enumerator.Current, current))
                {
                    found = true;
                    break;
                }
            }
            if (!found) throw new ArgumentException("The collection does not contain the current item.");
            return enumerator.MoveNext() ? enumerator.Current : default;
        }

        /// <summary>
        /// The merge modifies the target list so that all items equals the source list. This has some advantages over Clear and AddRange when
        /// the target list is bound to the UI or used by an ORM (Object Relational Mapping).
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="target">The target list.</param>
        /// <param name="source">The sort list.</param>
        /// <exception cref="ArgumentNullException">target and source must not be <c>null</c>.</exception>
        public static void Merge<T>(this IList<T> target, IReadOnlyList<T> source)
        {
            Merge(target, source, null, null, null, null, null);
        }

        /// <summary>
        /// The merge modifies the target list so that all items equals the source list. This has some advantages over Clear and AddRange when
        /// the target list is bound to the UI or used by an ORM (Object Relational Mapping).
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="target">The target list.</param>
        /// <param name="source">The sort list.</param>
        /// <param name="comparer">Optional, a custom comparer can be provided.</param>
        /// <exception cref="ArgumentNullException">target and source must not be <c>null</c>.</exception>
        public static void Merge<T>(this IList<T> target, IReadOnlyList<T> source, IEqualityComparer<T>? comparer)
        {
            Merge(target, source, comparer, null, null, null, null);
        }

        /// <summary>
        /// The merge modifies the target list so that all items equals the source list. This has some advantages over Clear and AddRange when
        /// the target list is bound to the UI or used by an ORM (Object Relational Mapping).
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="target">The target list.</param>
        /// <param name="source">The sort list.</param>
        /// <param name="comparer">Optional, a custom comparer can be provided.</param>
        /// <param name="insertAction">Optional, a custom action can be provided that is called for inserts.</param>
        /// <param name="removeAtAction">Optional, a custom action can be provided that is called for remove at.</param>
        /// <param name="resetAction">Optional, a custom action can be provided that is called for reset.</param>
        /// <exception cref="ArgumentNullException">target and source must not be <c>null</c>.</exception>
        public static void Merge<T>(this IList<T> target, IReadOnlyList<T> source, IEqualityComparer<T>? comparer,
            Action<int, T>? insertAction, Action<int>? removeAtAction, Action? resetAction)
        {
            Merge(target, source, comparer, insertAction, removeAtAction, resetAction, null);
        }

        /// <summary>
        /// The merge modifies the target list so that all items equals the source list. This has some advantages over Clear and AddRange when
        /// the target list is bound to the UI or used by an ORM (Object Relational Mapping).
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="target">The target list.</param>
        /// <param name="source">The sort list.</param>
        /// <param name="comparer">Optional, a custom comparer can be provided.</param>
        /// <param name="insertAction">Optional, a custom action can be provided that is called for inserts.</param>
        /// <param name="removeAtAction">Optional, a custom action can be provided that is called for remove at.</param>
        /// <param name="resetAction">Optional, a custom action can be provided that is called for reset.</param>
        /// <param name="moveAction">Optional, a custom action can be provided that is called for move.</param>
        /// <exception cref="ArgumentNullException">target and source must not be <c>null</c>.</exception>
        public static void Merge<T>(this IList<T> target, IReadOnlyList<T> source, IEqualityComparer<T>? comparer,
            Action<int, T>? insertAction, Action<int>? removeAtAction, Action? resetAction, Action<int, int>? moveAction)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (source == null) throw new ArgumentNullException(nameof(source));
            comparer ??= EqualityComparer<T>.Default;
            if (target.SequenceEqual(source, comparer)) return;

            insertAction ??= target.Insert;
            removeAtAction ??= target.RemoveAt;
            resetAction ??= (() =>
            {
                foreach (var item in target.ToArray()) { target.Remove(item); }  // Avoid Clear because of CollectionChanged events
                foreach (var item in source) { target.Add(item); }
            });

            // Item(s) added or removed
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
            else if (moveAction != null)  // Item(s) moved
            {
                var count = target.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!comparer.Equals(target[i], source[i]))
                    {
                        if (i + 1 < count && comparer.Equals(target[i + 1], source[i]))
                        {
                            int newIndex = -1;
                            T item = target[i];
                            for (int s = i + 1; s < count; s++)
                            {
                                if (comparer.Equals(source[s], item))
                                {
                                    newIndex = s;
                                    break;
                                }
                            }
                            if (newIndex < 0)
                            {
                                // Item was replaced instead of moved
                                resetAction();
                                return;
                            }
                            for (int j = i + 1; j < count; j++)
                            {
                                if (!comparer.Equals(source[j <= newIndex ? j - 1 : j], target[j]))
                                {
                                    // Second move operation is not supported -> Reset
                                    resetAction();
                                    return;
                                }
                            }
                            moveAction(i, newIndex);
                            return;
                        }
                        else
                        {
                            int oldIndex = -1;
                            T item = source[i];
                            for (int t = i + 1; t < count; t++)
                            {
                                if (comparer.Equals(target[t], item))
                                {
                                    oldIndex = t;
                                    break;
                                }
                            }
                            if (oldIndex < 0)
                            {
                                // Item was replaced instead of moved
                                resetAction();
                                return;
                            }
                            for (int j = i + 1; j < count; j++)
                            {
                                if (!comparer.Equals(target[j <= oldIndex ? j - 1 : j], source[j]))
                                {
                                    // Second move operation is not supported -> Reset
                                    resetAction();
                                    return;
                                }
                            }
                            moveAction(oldIndex, i);
                            return;
                        }
                    }
                }
            }

            resetAction();
        }
    }
}
