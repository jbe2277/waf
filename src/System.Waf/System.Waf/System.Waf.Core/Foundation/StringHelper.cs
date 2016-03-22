namespace System.Waf.Foundation
{
    /// <summary>
    /// Provides helper methods for working with strings.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Truncates the string to the specified maximum length.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>The truncated string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">maxLength is less than 0.</exception>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
