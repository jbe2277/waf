using System.Globalization;
using System.Windows.Data;

namespace System.Waf.Presentation.Converters
{
    /// <summary>
    /// Value converter that converts an object into a formatted string. The format specification is passed via the 
    /// ConverterParameter property.
    /// </summary>
    public sealed class StringFormatConverter : IValueConverter, IMultiValueConverter
    {
        /// <summary>Gets the default instance of this converter.</summary>
        public static StringFormatConverter Default { get; } = new StringFormatConverter();

        /// <summary>Converts an object into a formatted string.</summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="targetType">The type of the binding target property. This parameter will be ignored.</param>
        /// <param name="parameter">The format specification used to format the object.</param>
        /// <param name="culture">The culture to use in the converter. This parameter will be ignored.</param>
        /// <returns>The formatted string.</returns>
        public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return string.Format(CultureInfo.CurrentCulture, parameter as string ?? "{0}", value);
        }

        /// <summary>This method is not supported and throws an exception when it is called.</summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Nothing because this method throws an exception.</returns>
        /// <exception cref="NotSupportedException">Throws this exception when the method is called.</exception>
        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>Converts multiple objects into a formatted string.</summary>
        /// <param name="values">The objects to convert.</param>
        /// <param name="targetType">The type of the binding target property. This parameter will be ignored.</param>
        /// <param name="parameter">The format specification used to format the object.</param>
        /// <param name="culture">The culture to use in the converter. This parameter will be ignored.</param>
        /// <returns>The formatted string.</returns>
        public object Convert(object[] values, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return string.Format(CultureInfo.CurrentCulture, parameter as string ?? "{0}", values);
        }

        /// <summary>This method is not supported and throws an exception when it is called.</summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetTypes">The types to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Nothing because this method throws an exception.</returns>
        /// <exception cref="NotSupportedException">Throws this exception when the method is called.</exception>
        public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
        {
            throw new NotSupportedException();
        }
    }
}
