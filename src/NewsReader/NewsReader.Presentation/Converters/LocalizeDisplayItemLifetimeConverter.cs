using System;
using System.Globalization;
using Waf.NewsReader.Applications.DataModels;
using Xamarin.Forms;

namespace Waf.NewsReader.Presentation.Converters
{
    public class LocalizeDisplayItemLifetimeConverter : IValueConverter
    {
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            var lifetime = (DisplayItemLifetime)value!;
            switch (lifetime)
            {
                case DisplayItemLifetime._1Month: return "1 Month";
                case DisplayItemLifetime._3Month: return "3 Month";
                case DisplayItemLifetime._6Month: return "6 Month";
                case DisplayItemLifetime._1Year: return "1 Year";
                case DisplayItemLifetime.Forever: return "Forever";
                default: throw new InvalidOperationException($"{nameof(DisplayItemLifetime)} is not supported: {lifetime}");
            }
        }

        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotSupportedException();
        }
    }
}
