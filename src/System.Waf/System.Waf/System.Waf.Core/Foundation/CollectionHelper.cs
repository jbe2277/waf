using System.Collections.Generic;

namespace System.Waf.Foundation
{
    /// <summary>
    /// Provides helper methods for collections.
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// Returns an empty <see cref="IReadOnlyList{TResult}"/> that has the specified type argument.
        /// </summary>
        /// <typeparam name="TResult">The type to assign to the type parameter of the returned generic <see cref="IReadOnlyList{TResult}"/>.</typeparam>
        /// <returns>An empty <see cref="IReadOnlyList{TResult}"/> whose type argument is TResult.</returns>
        /// <remarks>The Empty{TResult}() method caches an empty sequence of type TResult. Same implementation as Array.Empty{T}.</remarks>
        public static IReadOnlyList<TResult> Empty<TResult>()
        {
            return EmptyCollection<TResult>.Instance;
        }

        private static class EmptyCollection<TElement>
        {
            internal static readonly TElement[] Instance = new TElement[0];
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire collection.
        /// </summary>
        /// <typeparam name="T">The collection item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The object to locate in the collection. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire collection, if found; otherwise, –1.</returns>
        public static int IndexOf<T>(this IEnumerable<T> collection, T item)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var list = collection as IList<T>;
            if (list != null) return list.IndexOf(item);

            int i = 0;
            foreach (T localItem in collection)
            {
                if (Equals(localItem, item)) return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Gets the next element in the collection or default when no next element can be found.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="current">The current item.</param>
        /// <returns>The next element in the collection or default when no next element can be found.</returns>
        /// <exception cref="ArgumentNullException">collection must not be <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The collection does not contain the specified current item.</exception>
        public static T GetNextElementOrDefault<T>(IEnumerable<T> collection, T current)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            
            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
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
                
                return enumerator.MoveNext() ? enumerator.Current : default(T);
            }
        }
    }
}
