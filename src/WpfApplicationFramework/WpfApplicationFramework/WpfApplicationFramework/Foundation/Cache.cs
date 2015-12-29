namespace System.Waf.Foundation
{
    /// <summary>
    /// Provides support for caching a value.
    /// </summary>
    /// <typeparam name="T">The type of object that is being cached.</typeparam>
    public sealed class Cache<T>
    {
        private readonly Func<T> valueFactory;
        private T value;
        private bool isDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cache{T}"/> class.
        /// </summary>
        /// <param name="valueFactory">The delegate that is invoked to produce the value when it is needed.</param>
        public Cache(Func<T> valueFactory)
        {
            if (valueFactory == null) { throw new ArgumentNullException(nameof(valueFactory)); }
            this.valueFactory = valueFactory;
            this.isDirty = true;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value
        {
            get
            {
                if (isDirty)
                {
                    value = valueFactory();
                    isDirty = false;
                }
                return value;
            }
        }

        /// <summary>
        /// Indicates that the Cache is dirty and will update itself at the next Value read.
        /// </summary>
        public bool IsDirty { get { return isDirty; } }

        /// <summary>
        /// Sets the Cache dirty. This ensures that the Cache is updated at the next Value read.
        /// </summary>
        public void SetDirty()
        {
            isDirty = true;
        }
    }
}
