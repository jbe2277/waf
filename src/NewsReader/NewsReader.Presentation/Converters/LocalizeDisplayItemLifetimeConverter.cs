using System.Globalization;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Presentation.Properties;

namespace Waf.NewsReader.Presentation.Converters;

public class LocalizeDisplayItemLifetimeConverter : IValueConverter
{
    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture) => value switch
    {
        DisplayItemLifetime._1Month => Resources.OneMonth,
        DisplayItemLifetime._3Month => Resources.ThreeMonth,
        DisplayItemLifetime._6Month => Resources.SixMonth,
        DisplayItemLifetime._1Year => Resources.OneYear,
        DisplayItemLifetime.Forever => Resources.Unlimited,
        _ => throw new InvalidOperationException($"{nameof(DisplayItemLifetime)} is not supported: {value}"),
    };

    public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotSupportedException();
}
