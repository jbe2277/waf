using System.Globalization;

namespace Waf.NewsReader.Presentation.Converters;

public class MarkAsReadConverter : IValueConverter
{
    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if (value is not bool markAsRead) return null;
        return markAsRead ? "Unread" : "Read";
    }

    public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotSupportedException();
}
