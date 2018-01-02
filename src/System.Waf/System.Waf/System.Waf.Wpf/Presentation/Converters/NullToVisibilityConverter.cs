#if WINDOWS_UWP
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
#else
    using System.Globalization;    
    using System.Windows;
    using System.Windows.Data;
#endif

namespace System.Waf.Presentation.Converters
{
    /// <summary>
    /// Value converter that uses the is Null condition to return the associated Visibility enumeration value.
    /// </summary>
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Gets the default instance of this converter.
        /// </summary>
        public static NullToVisibilityConverter Default { get; } = new NullToVisibilityConverter();


        /// <summary>
        /// Returns a Visibility enumeration value depending on the is Null condition of the specified value.
        /// </summary>
        /// <param name="value">Object which is checked for null.</param>
        /// <param name="targetType">The type of the binding target property. This parameter will be ignored.</param>
        /// <param name="parameter">Use the string 'Invert' to get an inverted result (Visible and Collapsed are exchanged). 
        /// Do not specify this parameter if the default behavior is desired.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Visible when the value is not null; otherwise Collapsed.</returns>
#if WINDOWS_UWP
        public object Convert(object value, Type targetType, object parameter, string culture)
#else
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            bool invert = string.Equals(parameter as string, "invert", StringComparison.OrdinalIgnoreCase);
            if (invert)
            {
                return value != null ? Visibility.Collapsed : Visibility.Visible;
            }
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// This method is not supported and throws an exception when it is called.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Nothing because this method throws an exception.</returns>
        /// <exception cref="NotSupportedException">Throws this exception when the method is called.</exception>
#if WINDOWS_UWP
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
#else
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            throw new NotSupportedException();
        }
    }
}
