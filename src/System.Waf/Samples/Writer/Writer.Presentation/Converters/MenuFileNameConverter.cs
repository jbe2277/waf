using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Waf.Writer.Presentation.Converters;

public class MenuFileNameConverter : IValueConverter
{
    private const int MaxCharacters = 40;

    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if (value is not string fileName || string.IsNullOrEmpty(fileName)) return "";
        fileName = Path.GetFileName(fileName);
        return fileName.Length <= MaxCharacters ? fileName : fileName.Remove(MaxCharacters - 3) + "...";
    }

    public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotSupportedException();
}
