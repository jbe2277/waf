using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Presentation.Services;
using System;
using Windows.UI.Xaml.Data;

namespace Jbe.NewsReader.Presentation.Converters
{
    public class LocalizeDisplayAppThemeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var theme = (DisplayAppTheme)value;
            switch (theme)
            {
                case DisplayAppTheme.Auto:
                    return ResourceService.GetString("AppThemeAuto");
                case DisplayAppTheme.Light:
                    return ResourceService.GetString("AppThemeLight");
                case DisplayAppTheme.Dark:
                    return ResourceService.GetString("AppThemeDark");
                default:
                    throw new InvalidOperationException($"Theme is not supported: {theme}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
