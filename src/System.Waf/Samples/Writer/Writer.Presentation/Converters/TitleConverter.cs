using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Waf.Writer.Presentation.Properties;

namespace Waf.Writer.Presentation.Converters
{
    public class TitleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || !(values[0] is string) || !(values[1] == null || values[1] is string))
            {
                return DependencyProperty.UnsetValue;
            }

            var title = (string)values[0];
            var documentName = (string)values[1];

            if (!string.IsNullOrEmpty(documentName))
            {
                title = string.Format(culture, Resources.TitleFormatString, documentName, title);
            }

            return title;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
