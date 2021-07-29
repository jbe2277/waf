using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Waf.Writer.Presentation.Converters
{
    public class PercentConverter : IValueConverter
    {
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture) => string.Format(culture, "{0:P0}", value);

        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            culture ??= CultureInfo.CurrentCulture;
            return value is string s && double.TryParse(s.Replace(culture.NumberFormat.PercentSymbol, ""), NumberStyles.Float | NumberStyles.AllowThousands, culture, out var d)
                ? d / 100d
                : DependencyProperty.UnsetValue;
        }
    }
}
