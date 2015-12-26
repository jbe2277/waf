using System.Runtime.Serialization;

namespace System.Waf.UnitTesting
{
    /// <summary>
    /// Represents assertion errors that occur at runtime.
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected AssertException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { 
        }
    }
}
