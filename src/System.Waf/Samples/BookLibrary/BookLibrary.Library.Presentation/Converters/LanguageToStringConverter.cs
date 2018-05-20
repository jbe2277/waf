using System;
using System.Globalization;
using System.Windows.Data;
using Waf.BookLibrary.Library.Domain;
using Waf.BookLibrary.Library.Presentation.Properties;

namespace Waf.BookLibrary.Library.Presentation.Converters
{
    public class LanguageToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Language)) { return null; }
            
            var language = (Language)value;
            switch (language)
            {
                case Language.Undefined:
                    return Resources.Undefined;
                case Language.English:
                    return Resources.English;
                case Language.German:
                    return Resources.German;
                case Language.French:
                    return Resources.French;
                case Language.Spanish:
                    return Resources.Spanish;
                case Language.Chinese:
                    return Resources.Chinese;
                case Language.Japanese:
                    return Resources.Japanese;
            }
            throw new InvalidOperationException("Enum value is unknown.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
