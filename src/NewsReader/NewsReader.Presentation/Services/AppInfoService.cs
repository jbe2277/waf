using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services;

public class AppInfoService : IAppInfoService
{
    public string AppName { get; } = AppInfo.Name;

    public string VersionString { get; } = AppInfo.VersionString;
}
