using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Presentation.Services;
using System;
using Windows.UI.Xaml.Data;

namespace Jbe.NewsReader.Presentation.Converters
{
    public class LocalizeDisplayMaxItemsLimitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var lifetime = (DisplayMaxItemsLimit)value;
            return lifetime.ToValue()?.ToString() ?? ResourceService.GetString("MaxItemsLimitUnlimited");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
