using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Waf.NewsReader.Presentation.Converters;

public class ErrorsToMessageConverter : IValueConverter
{
    public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if (!(value is IEnumerable<ValidationResult> errors)) return null;
        return string.Join(Environment.NewLine, errors.Where(x => x.MemberNames.Any(y => y.Equals(parameter))).Select(x => x.ErrorMessage));
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotSupportedException();
    }
}
