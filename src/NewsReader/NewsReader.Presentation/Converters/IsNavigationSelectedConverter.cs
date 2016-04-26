using Jbe.NewsReader.Applications.ViewModels;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace Jbe.NewsReader.Presentation.Converters
{
    public class IsNavigationSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var navigationItems = ((string)parameter).Split(';').Select(x => Enum.Parse(typeof(NavigationItem), x.Trim()));
            return navigationItems.Any(x => x.Equals(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
