using Waf.NewsReader.Applications.Services;
using Xamarin.Essentials;

namespace Waf.NewsReader.Presentation.Services
{
    public class AppInfoService : IAppInfoService
    {
        public AppInfoService()
        {
            AppName = AppInfo.Name;
            VersionString = AppInfo.VersionString + "." + AppInfo.BuildString;
        }

        public string AppName { get; }

        public string VersionString { get; }
    }
}
