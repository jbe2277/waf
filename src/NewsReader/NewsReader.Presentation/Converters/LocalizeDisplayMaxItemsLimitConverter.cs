using System.Globalization;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Presentation.Properties;

namespace Waf.NewsReader.Presentation.Converters;

public class LocalizeDisplayMaxItemsLimitConverter : IValueConverter
{
    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        var lifetime = (DisplayMaxItemsLimit)value!;
        return lifetime.ToValue()?.ToString(culture) ?? Resources.Unlimited;
    }

    public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotSupportedException();
}
