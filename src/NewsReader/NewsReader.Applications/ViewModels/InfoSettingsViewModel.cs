using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Windows.ApplicationModel;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class InfoSettingsViewModel : ViewModel<IInfoSettingsView>
    {
        [ImportingConstructor]
        public InfoSettingsViewModel(IInfoSettingsView view) : base(view)
        {
        }


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

        public ICommand LaunchWindowsStoreCommand { get; set; }
    }
}
