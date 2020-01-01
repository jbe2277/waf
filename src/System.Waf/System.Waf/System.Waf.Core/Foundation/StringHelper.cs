namespace System.Waf.Foundation
{
    /// <summary>
    /// Provides helper methods for working with strings.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Returns a value indicating whether a specified substring occurs within this string.
        /// </summary>
        /// <param name="s">The string to search for the value.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>true if the value parameter occurs within this string, or if value is the empty string (""); otherwise, false.</returns>
        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            return s.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// Truncates the string to the specified maximum length.
        /// </summary>
        /// <param name="s">The string to truncate.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>The truncated string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">maxLength is less than 0.</exception>
        public static string? Truncate(this string? s, int maxLength)
        {
            if (string.IsNullOrEmpty(s)) { return s; }
            return s!.Length <= maxLength ? s : s!.Substring(0, maxLength);
        }
    }
}
