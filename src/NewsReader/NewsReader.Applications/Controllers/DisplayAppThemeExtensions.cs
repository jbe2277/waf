using Jbe.NewsReader.Applications.ViewModels;
using System;

namespace Jbe.NewsReader.Applications.Controllers
{
    public static class DisplayAppThemeExtensions
    {
        public static DisplayAppTheme FromSettings(int? theme)
        {
            switch (theme)
            {
                case null:
                    return DisplayAppTheme.Auto;
                case 0:
                    return DisplayAppTheme.Light;
                case 1:
                    return DisplayAppTheme.Dark;
                default:
                    throw new NotSupportedException($"This theme is not supported: {theme}");
            }
        }

        public static int? ToSettings(this DisplayAppTheme theme)
        {
            switch (theme)
            {
                case DisplayAppTheme.Auto:
                    return null;
                case DisplayAppTheme.Light:
                    return 0;
                case DisplayAppTheme.Dark:
                    return 1;
                default:
                    throw new NotSupportedException($"This theme is not supported: {theme}");
            }
        }
    }
}
