using Jbe.NewsReader.Applications.ViewModels;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace Jbe.NewsReader.Presentation.Converters
{
    public class LocalizeDisplayMaxItemsLimitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var lifetime = (DisplayMaxItemsLimit)value;
            return (object)lifetime.ToValue() ?? ResourceLoader.GetForViewIndependentUse().GetString("MaxItemsLimitUnlimited");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
