using Jbe.NewsReader.Applications.ViewModels;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace Jbe.NewsReader.Presentation.Converters
{
    public class LocalizeAppThemeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var theme = (AppTheme)value;
            switch (theme)
            {
                case AppTheme.Auto:
                    return ResourceLoader.GetForViewIndependentUse().GetString("AppThemeAuto");
                case AppTheme.Light:
                    return ResourceLoader.GetForViewIndependentUse().GetString("AppThemeLight");
                case AppTheme.Dark:
                    return ResourceLoader.GetForViewIndependentUse().GetString("AppThemeDark");
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
