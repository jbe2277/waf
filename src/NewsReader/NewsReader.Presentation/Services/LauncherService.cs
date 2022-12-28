using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services;

public class LauncherService : ILauncherService
{
    public Task LaunchBrowser(Uri uri) => Browser.OpenAsync(uri, BrowserLaunchMode.External);
}
