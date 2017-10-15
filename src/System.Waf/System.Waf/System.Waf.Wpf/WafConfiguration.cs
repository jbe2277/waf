using System.Windows;
using System.ComponentModel;

namespace System.Waf
{
    /// <summary>
    /// Configuration settings for the Win Application Framework (WAF).
    /// </summary>
    public static class WafConfiguration
    {
        /// <summary>
        /// Gets a value indicating whether the code is running in design mode.
        /// </summary>
        /// <value><c>true</c> if the code is running in design mode; otherwise, <c>false</c>.</value>
        public static bool IsInDesignMode { get; } = DesignerProperties.GetIsInDesignMode(new DependencyObject());
    }
}
