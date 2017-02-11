using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Presentation.Services;
using System;
using Windows.UI.Xaml.Data;

namespace Jbe.NewsReader.Presentation.Converters
{
    public class LocalizeDisplayItemLifetimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var lifetime = (DisplayItemLifetime)value;
            switch (lifetime)
            {
                case DisplayItemLifetime._1Month:
                    return ResourceService.GetString("ItemLifetime1Month");
                case DisplayItemLifetime._3Month:
                    return ResourceService.GetString("ItemLifetime3Month");
                case DisplayItemLifetime._6Month:
                    return ResourceService.GetString("ItemLifetime6Month");
                case DisplayItemLifetime._1Year:
                    return ResourceService.GetString("ItemLifetime1Year");
                case DisplayItemLifetime.Forever:
                    return ResourceService.GetString("ItemLifetimeForever");
                default:
                    throw new InvalidOperationException($"DisplayItemLifetime is not supported: {lifetime}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
