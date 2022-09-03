using System.Globalization;
using Waf.NewsReader.Applications.DataModels;

namespace Waf.NewsReader.Presentation.Converters
{
    public class LocalizeDisplayItemLifetimeConverter : IValueConverter
    {
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            var lifetime = (DisplayItemLifetime)value!;
            return lifetime switch
            {
                DisplayItemLifetime._1Month => "1 Month",
                DisplayItemLifetime._3Month => "3 Month",
                DisplayItemLifetime._6Month => "6 Month",
                DisplayItemLifetime._1Year => "1 Year",
                DisplayItemLifetime.Forever => "Forever",
                _ => throw new InvalidOperationException($"{nameof(DisplayItemLifetime)} is not supported: {lifetime}"),
            };
        }

        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotSupportedException();
        }
    }
}
