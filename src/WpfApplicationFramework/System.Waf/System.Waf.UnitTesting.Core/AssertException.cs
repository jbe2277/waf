namespace System.Waf.UnitTesting
{
    /// <summary>
    /// Represents assertion errors that occur at runtime.
    /// </summary>
    public class AssertException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertException"/> class.
        /// </summary>
        public AssertException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AssertException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public AssertException(string message, Exception inner) : base(message, inner) { }
    }
}
