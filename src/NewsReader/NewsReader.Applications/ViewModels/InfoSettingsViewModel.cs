using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.Applications;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class InfoSettingsViewModel : ViewModelCore<IInfoSettingsView>
    {
        [ImportingConstructor]
        public InfoSettingsViewModel(IInfoSettingsView view, IAppInfoService appInfoService) : base(view)
        {
            AppName = appInfoService.AppName;
            AppVersion = appInfoService.AppVersion;
            AppDescription = appInfoService.AppDescription;
            AppPublisherName = appInfoService.AppPublisherName;
        }


        public string AppName { get; }
        
        public string AppVersion { get; }
        
        public string AppDescription { get; }

        public string AppPublisherName { get; }

        public ICommand LaunchWindowsStoreCommand { get; set; }

        public ICommand EnableDeveloperSettingsCommand { get; set; }
    }
}
