using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services
{
    public class AppInfoService : IAppInfoService
    {
        public AppInfoService()
        {
            AppName = AppInfo.Name;
            VersionString = AppInfo.VersionString;
        }

        public string AppName { get; }

        public string VersionString { get; }
    }
}
