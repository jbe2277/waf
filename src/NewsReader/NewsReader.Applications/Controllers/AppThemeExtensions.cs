using Jbe.NewsReader.Applications.ViewModels;
using System;
using Windows.UI.Xaml;

namespace Jbe.NewsReader.Applications.Controllers
{
    public static class AppThemeExtensions
    {
        public static AppTheme ToAppTheme(this ApplicationTheme? theme)
        {
            switch (theme)
            {
                case null:
                    return AppTheme.Auto;
                case ApplicationTheme.Light:
                    return AppTheme.Light;
                case ApplicationTheme.Dark:
                    return AppTheme.Dark;
                default:
                    throw new NotSupportedException($"This ApplicationTheme is not supported: {theme}");
            }
        }

        public static ApplicationTheme? ToApplicationTheme(this AppTheme theme)
        {
            switch (theme)
            {
                case AppTheme.Auto:
                    return null;
                case AppTheme.Light:
                    return ApplicationTheme.Light;
                case AppTheme.Dark:
                    return ApplicationTheme.Dark;
                default:
                    throw new NotSupportedException($"This AppTheme is not supported: {theme}");
            }
        }
    }
}
