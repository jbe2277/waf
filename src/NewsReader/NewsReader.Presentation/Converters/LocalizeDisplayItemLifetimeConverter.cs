using Jbe.NewsReader.Applications.ViewModels;
using System;
using Windows.ApplicationModel.Resources;
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
                    return ResourceLoader.GetForViewIndependentUse().GetString("ItemLifetime1Month");
                case DisplayItemLifetime._3Month:
                    return ResourceLoader.GetForViewIndependentUse().GetString("ItemLifetime3Month");
                case DisplayItemLifetime._6Month:
                    return ResourceLoader.GetForViewIndependentUse().GetString("ItemLifetime6Month");
                case DisplayItemLifetime._1Year:
                    return ResourceLoader.GetForViewIndependentUse().GetString("ItemLifetime1Year");
                case DisplayItemLifetime.Forever:
                    return ResourceLoader.GetForViewIndependentUse().GetString("ItemLifetimeForever");
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
