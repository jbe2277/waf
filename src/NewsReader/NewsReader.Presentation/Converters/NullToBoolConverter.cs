using System.Globalization;

namespace Waf.NewsReader.Presentation.Converters
{
    public class NullToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (string.Equals(parameter as string, "invert", StringComparison.OrdinalIgnoreCase))
            {
                return value == null;
            }
            return value != null;
        }

        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotSupportedException();
        }
    }
}
