using System.Globalization;
using System.Windows.Data;
using Waf.BookLibrary.Library.Domain;
using Waf.BookLibrary.Library.Presentation.Properties;

namespace Waf.BookLibrary.Library.Presentation.Converters
{
    public class LanguageToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is not Language language) return null;
            return language switch
            {
                Language.Undefined => Resources.Undefined,
                Language.English => Resources.English,
                Language.German => Resources.German,
                Language.French => Resources.French,
                Language.Spanish => Resources.Spanish,
                Language.Chinese => Resources.Chinese,
                Language.Japanese => Resources.Japanese,
                _ => throw new InvalidOperationException("Enum value is unknown."),
            };
        }

        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotSupportedException();
    }
}
