using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Waf.Writer.Presentation.Converters
{
    public class MenuFileNameConverter : IValueConverter
    {
        private const int MaxCharacters = 40;
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fileName = value as string;
            if (!string.IsNullOrEmpty(fileName))
            {
                fileName = Path.GetFileName(fileName);
                
                if (fileName.Length <= MaxCharacters)
                {
                    return fileName;
                }
                else
                {
                    return fileName.Remove(MaxCharacters - 3) + "...";
                }
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
