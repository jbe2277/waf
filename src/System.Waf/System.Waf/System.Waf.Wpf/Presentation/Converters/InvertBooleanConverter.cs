#if WINDOWS_UWP
    using Windows.UI.Xaml.Data;
#else    
    using System.Globalization;
    using System.Windows.Data;
#endif

namespace System.Waf.Presentation.Converters
{
    /// <summary>
    /// Value converter that inverts a boolean value.
    /// </summary>
    public sealed class InvertBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Gets the default instance of this converter.
        /// </summary>
        public static InvertBooleanConverter Default { get; } = new InvertBooleanConverter();


        /// <summary>
        /// Converts a boolean value into the inverted value.
        /// </summary>
        /// <param name="value">The boolean value to invert.</param>
        /// <param name="targetType">The type of the binding target property. This parameter will be ignored.</param>
        /// <param name="parameter">The converter parameter to use. This parameter will be ignored.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The inverter boolean value.</returns>
#if WINDOWS_UWP
        public object Convert(object value, Type targetType, object parameter, string culture)
#else
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            return value == null ? (object)null : !(bool)value;
        }

        /// <summary>
        /// Converts a boolean value into the inverted value.
        /// </summary>
        /// <param name="value">The boolean value to invert.</param>
        /// <param name="targetType">The type to convert to. This parameter will be ignored.</param>
        /// <param name="parameter">The converter parameter to use. This parameter will be ignored.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The inverter boolean value.</returns>
#if WINDOWS_UWP
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
#else
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            return value == null ? (object)null : !(bool)value;
        }
    }
}
