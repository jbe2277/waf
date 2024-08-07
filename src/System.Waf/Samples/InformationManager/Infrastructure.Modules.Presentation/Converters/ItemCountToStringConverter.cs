﻿using System.Windows.Data;
using System.Globalization;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Converters;

public class ItemCountToStringConverter : IValueConverter
{
    public static ItemCountToStringConverter Default { get; } = new();

    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if (value == null) return "";
        return string.Format(culture, "  {0}", value);
    }

    public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotSupportedException();
}
