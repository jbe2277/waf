using System;
using System.Globalization;
using Xamarin.Forms;

namespace Waf.NewsReader.Presentation.Converters
{
    public class MarkAsReadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool markAsRead)) return null;
            return markAsRead ? "Mark as unread" : "Mark as read";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
