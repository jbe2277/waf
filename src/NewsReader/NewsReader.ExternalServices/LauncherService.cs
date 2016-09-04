using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;
using Windows.ApplicationModel;
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

        public async Task<bool> LaunchStoreAsync()
        {
            // https://msdn.microsoft.com/en-us/library/windows/apps/mt228343.aspx
            return await Launcher.LaunchUriAsync(new Uri($"ms-windows-store:pdp?PFN={Package.Current.Id.FamilyName}"));
        }

        public async Task<bool> LaunchReviewAsync()
        {
            // https://msdn.microsoft.com/en-us/library/windows/apps/mt228343.aspx
            return await Launcher.LaunchUriAsync(new Uri($"ms-windows-store:review?PFN={Package.Current.Id.FamilyName}"));
        }
    }
}
