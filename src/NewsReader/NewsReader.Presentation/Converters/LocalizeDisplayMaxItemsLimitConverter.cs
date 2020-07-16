using System;
using System.Globalization;
using Waf.NewsReader.Applications.DataModels;
using Xamarin.Forms;

namespace Waf.NewsReader.Presentation.Converters
{
    public class LocalizeDisplayMaxItemsLimitConverter : IValueConverter
    {
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            var lifetime = (DisplayMaxItemsLimit)value!;
            return lifetime.ToValue()?.ToString(CultureInfo.CurrentCulture) ?? "Unlimited";
        }

        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotSupportedException();
        }
    }
}
