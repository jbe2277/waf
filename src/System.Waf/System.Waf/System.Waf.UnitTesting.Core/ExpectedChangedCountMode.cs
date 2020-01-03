namespace System.Waf.UnitTesting
{
    /// <summary>Defines how the expected changed count is used for the assert condition.</summary>
    public enum ExpectedChangedCountMode
    {
        /// <summary>Expects the exact count of changed events.</summary>
        Exact,

        /// <summary>Expects at least the count of changed events.</summary>
        AtLeast,

        /// <summary>Expects at most the count of changed events.</summary>
        AtMost
    }
}
