using Jbe.NewsReader.Applications.Services;
using System.Composition;
using Windows.ApplicationModel;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(IAppInfoService)), Export, Shared]
    public class AppInfoService : IAppInfoService
    {
        public string AppName => Package.Current.DisplayName;

        public string AppVersion
        {
            get
            {
                PackageVersion version = Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public string AppDescription => Package.Current.Description;

        public string AppPublisherName => Package.Current.PublisherDisplayName;

        public string PackageFamilyName => Package.Current.Id.FamilyName;
    }
}
