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
        /// <remarks>The Empty{TResult}() method caches an empty sequence of type TResult.</remarks>
        public static IReadOnlyList<TResult> Empty<TResult>()
        {
            return EmptyCollection<TResult>.Instance;
        }

        private class EmptyCollection<TElement>
        {
            internal static readonly TElement[] Instance = new TElement[0];
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
            if (collection == null) { throw new ArgumentNullException(nameof(collection)); }

            bool found = false;
            IEnumerator<T> enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (EqualityComparer<T>.Default.Equals(enumerator.Current, current))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new ArgumentException("The collection does not contain the current item.");
            }

            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            else
            {
                return default(T);
            }
        }
    }
}
