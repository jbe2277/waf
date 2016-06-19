using Jbe.NewsReader.Applications.ViewModels;
using System;
using Windows.UI.Xaml;

namespace Jbe.NewsReader.Applications.Controllers
{
    public static class DisplayAppThemeExtensions
    {
        public static DisplayAppTheme ToAppTheme(this ApplicationTheme? theme)
        {
            switch (theme)
            {
                case null:
                    return DisplayAppTheme.Auto;
                case ApplicationTheme.Light:
                    return DisplayAppTheme.Light;
                case ApplicationTheme.Dark:
                    return DisplayAppTheme.Dark;
                default:
                    throw new NotSupportedException($"This ApplicationTheme is not supported: {theme}");
            }
        }

        public static ApplicationTheme? ToApplicationTheme(this DisplayAppTheme theme)
        {
            switch (theme)
            {
                case DisplayAppTheme.Auto:
                    return null;
                case DisplayAppTheme.Light:
                    return ApplicationTheme.Light;
                case DisplayAppTheme.Dark:
                    return ApplicationTheme.Dark;
                default:
                    throw new NotSupportedException($"This AppTheme is not supported: {theme}");
            }
        }
    }
}
