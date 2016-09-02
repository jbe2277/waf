using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;
using Windows.System;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(ILauncherService)), Export, Shared]
    public class LauncherService : ILauncherService
    {
        public async Task<bool> LaunchUriAsync(Uri uri)
        {
            return await Launcher.LaunchUriAsync(uri);
        }
    }
}
