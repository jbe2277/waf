using System;
using System.Threading.Tasks;
using Waf.NewsReader.Applications.Services;
using Xamarin.Essentials;

namespace Waf.NewsReader.Presentation.Services
{
    public class LauncherService : ILauncherService
    {
        public Task LaunchBrowser(Uri uri)
        {
            return Browser.OpenAsync(uri, BrowserLaunchMode.External);
        }
    }
}
