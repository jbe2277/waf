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
        public Task<bool> LaunchUriAsync(Uri uri)
        {
            return Launcher.LaunchUriAsync(uri).AsTask();
        }

        public Task<bool> LaunchStoreAsync()
        {
            // https://msdn.microsoft.com/en-us/library/windows/apps/mt228343.aspx
            return Launcher.LaunchUriAsync(new Uri($"ms-windows-store:pdp?PFN={Package.Current.Id.FamilyName}")).AsTask();
        }

        public Task<bool> LaunchReviewAsync()
        {
            // https://msdn.microsoft.com/en-us/library/windows/apps/mt228343.aspx
            return Launcher.LaunchUriAsync(new Uri($"ms-windows-store:review?PFN={Package.Current.Id.FamilyName}")).AsTask();
        }
    }
}
