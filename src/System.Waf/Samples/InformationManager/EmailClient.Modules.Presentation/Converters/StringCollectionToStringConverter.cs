using System.Windows.Data;
using System.Globalization;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Converters;

public class StringCollectionToStringConverter : IValueConverter
{
    public static StringCollectionToStringConverter Default { get; } = new();

    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture) => string.Join("; ", (IEnumerable<string>)value!);

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotSupportedException();
}
