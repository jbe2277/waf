using System.Diagnostics.CodeAnalysis;

namespace System.Waf.Foundation
{
    /// <summary>Provides support for caching a value.</summary>
    /// <typeparam name="T">The type of object that is being cached.</typeparam>
    public sealed class Cache<T>
    {
        private readonly Func<T> valueFactory;
        private T value = default!;

        /// <summary>Initializes a new instance of the <see cref="Cache{T}"/> class.</summary>
        /// <param name="valueFactory">The delegate that is invoked to produce the value when it is needed.</param>
        public Cache(Func<T> valueFactory)
        {
            this.valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
            IsDirty = true;
        }

        /// <summary>Gets the value.</summary>
        [AllowNull]
        public T Value
        {
            get
            {
                if (IsDirty)
                {
                    value = valueFactory();
                    IsDirty = false;
                }
                return value;
            }
        }

        /// <summary>Indicates that the Cache is dirty and will update itself at the next Value read.</summary>
        public bool IsDirty { get; private set; }

        /// <summary>Sets the Cache dirty. This ensures that the Cache is updated at the next Value read.</summary>
        public void SetDirty()
        {
            IsDirty = true;
        }
    }
}
