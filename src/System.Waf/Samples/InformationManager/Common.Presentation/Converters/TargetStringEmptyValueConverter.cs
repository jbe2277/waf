using System.Globalization;
using System.Windows.Data;

namespace Waf.InformationManager.Common.Presentation.Converters
{
    /// <summary>Value converter that returns a special string if the (string) value is null or empty.</summary>
    public class TargetStringEmptyValueConverter : IValueConverter
    {
        /// <summary>Returns a special string if the (string) value is null or empty. Otherwise it returns value.</summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="targetType">The type of the binding target property. This parameter will be ignored.</param>
        /// <param name="parameter">This parameter will be ignored.</param>
        /// <param name="culture">The culture to use in the converter. This parameter will be ignored.</param>
        /// <returns>The converted string.</returns>
        public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture) => string.IsNullOrEmpty(value as string) ? "(none)" : value;

        /// <summary>This method is not supported and throws an exception when it is called.</summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Nothing because this method throws an exception.</returns>
        /// <exception cref="NotSupportedException">Throws this exception when the method is called.</exception>
        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotSupportedException();
    }
}
