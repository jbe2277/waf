using System.Globalization;
using System.Windows.Data;

namespace Waf.Writer.Presentation.Converters;

public class DoubleToZoomConverter : IValueConverter
{
    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture) => (double)value! * 100;

    public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => (double)value! / 100;
}
