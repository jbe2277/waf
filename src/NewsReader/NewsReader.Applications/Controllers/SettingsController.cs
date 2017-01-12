using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Domain;
using System;
using System.ComponentModel;
using System.Composition;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace Jbe.NewsReader.Applications.Controllers
{
    [Export, Shared]
    internal class SettingsController
    {
        private const string themeSettingsKey = "Theme";
        private readonly ILauncherService launcherService;
        private readonly IAppInfoService appInfoService;
        private readonly IAppDataService appDataService;
        private readonly Lazy<SettingsLayoutViewModel> settingsLayoutViewModel;
        private readonly Lazy<GeneralSettingsViewModel> generalSettingsViewModel;
        private readonly Lazy<InfoSettingsViewModel> infoSettingsViewModel;
        private readonly AsyncDelegateCommand launchWindowsStoreCommand;


        [ImportingConstructor]
        public SettingsController(ILauncherService launcherService, IAppInfoService appInfoService, IAppDataService appDataService, Lazy<SettingsLayoutViewModel> settingsLayoutViewModel, 
            Lazy<GeneralSettingsViewModel> generalSettingsViewModel, Lazy<InfoSettingsViewModel> infoSettingsViewModel)
        {
            this.launcherService = launcherService;
            this.appInfoService = appInfoService;
            this.appDataService = appDataService;
            this.settingsLayoutViewModel = new Lazy<SettingsLayoutViewModel>(() => InitializeSettingsLayoutViewModel(settingsLayoutViewModel));
            this.generalSettingsViewModel = new Lazy<GeneralSettingsViewModel>(() => InitializeGeneralSettingsViewModel(generalSettingsViewModel));
            this.infoSettingsViewModel = new Lazy<InfoSettingsViewModel>(() => InitializeInfoSettingsViewModel(infoSettingsViewModel));
            this.launchWindowsStoreCommand = new AsyncDelegateCommand(LaunchWindowsStore);
        }


        public FeedManager FeedManager { get; set; }

        public object SettingsView => settingsLayoutViewModel.Value.View;


        private SettingsLayoutViewModel InitializeSettingsLayoutViewModel(Lazy<SettingsLayoutViewModel> viewModel)
        {
            viewModel.Value.LazyGeneralSettingsView = new Lazy<object>(() => generalSettingsViewModel.Value.View);
            viewModel.Value.LazyInfoSettingsView = new Lazy<object>(() => infoSettingsViewModel.Value.View);
            return viewModel.Value;
        }

        private GeneralSettingsViewModel InitializeGeneralSettingsViewModel(Lazy<GeneralSettingsViewModel> viewModel)
        {
            viewModel.Value.SelectedAppTheme = DisplayAppThemeExtensions.FromSettings((int?)appDataService.LocalSettings[themeSettingsKey]);
            viewModel.Value.FeedManager = FeedManager;
            viewModel.Value.PropertyChanged += GeneralSettingsViewModelPropertyChanged;
            return viewModel.Value;
        }

        private InfoSettingsViewModel InitializeInfoSettingsViewModel(Lazy<InfoSettingsViewModel> viewModel)
        {
            viewModel.Value.LaunchWindowsStoreCommand = launchWindowsStoreCommand;
            return viewModel.Value;
        }

        private Task LaunchWindowsStore()
        {
            return launcherService.LaunchStoreAsync();
        }

        private void GeneralSettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GeneralSettingsViewModel.SelectedAppTheme))
            {
                appDataService.LocalSettings[themeSettingsKey] = generalSettingsViewModel.Value.SelectedAppTheme.ToSettings();
            }
        }
    }
}
