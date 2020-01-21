using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Waf.BookLibrary.Library.Presentation.Converters
{
    public class StringToUriConverter : IValueConverter
    {
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return Uri.TryCreate(value as string ?? "", UriKind.RelativeOrAbsolute, out Uri? uri) ? uri : DependencyProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is string s) { return s; }
            return ((Uri?)value)?.OriginalString;
        }
    }
}
