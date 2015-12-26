using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Waf.BookLibrary.Library.Presentation.Converters
{
    public class StringToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri;
            if (Uri.TryCreate(value as string ?? "", UriKind.RelativeOrAbsolute, out uri))
            {
                return uri;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            if (s != null) { return s; }

            return ((Uri)value).OriginalString;
        }
    }
}
