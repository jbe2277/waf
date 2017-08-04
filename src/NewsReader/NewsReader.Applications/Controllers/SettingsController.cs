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
        private readonly ILauncherService launcherService;
        private readonly IAppDataService appDataService;
        private readonly Lazy<SettingsLayoutViewModel> settingsLayoutViewModel;
        private readonly Lazy<GeneralSettingsViewModel> generalSettingsViewModel;
        private readonly Lazy<InfoSettingsViewModel> infoSettingsViewModel;
        private readonly Lazy<DeveloperSettingsViewModel> developerSettingsViewModel;
        private readonly AsyncDelegateCommand launchWindowsStoreCommand;
        private readonly DelegateCommand enableDeveloperSettingsCommand;


        [ImportingConstructor]
        public SettingsController(ILauncherService launcherService, IAppDataService appDataService, Lazy<SettingsLayoutViewModel> settingsLayoutViewModel, 
            Lazy<GeneralSettingsViewModel> generalSettingsViewModel, Lazy<InfoSettingsViewModel> infoSettingsViewModel, Lazy<DeveloperSettingsViewModel> developerSettingsViewModel)
        {
            this.launcherService = launcherService;
            this.appDataService = appDataService;
            this.settingsLayoutViewModel = new Lazy<SettingsLayoutViewModel>(() => InitializeSettingsLayoutViewModel(settingsLayoutViewModel));
            this.generalSettingsViewModel = new Lazy<GeneralSettingsViewModel>(() => InitializeGeneralSettingsViewModel(generalSettingsViewModel));
            this.infoSettingsViewModel = new Lazy<InfoSettingsViewModel>(() => InitializeInfoSettingsViewModel(infoSettingsViewModel));
            this.developerSettingsViewModel = new Lazy<DeveloperSettingsViewModel>(() => InitializeDeveloperSettingsViewModel(developerSettingsViewModel));
            launchWindowsStoreCommand = new AsyncDelegateCommand(LaunchWindowsStore);
            enableDeveloperSettingsCommand = new DelegateCommand(() => settingsLayoutViewModel.Value.DeveloperSettingsEnabled = true);
        }


        public FeedManager FeedManager { get; set; }

        public object SettingsView => settingsLayoutViewModel.Value.View;


        private SettingsLayoutViewModel InitializeSettingsLayoutViewModel(Lazy<SettingsLayoutViewModel> viewModel)
        {
            viewModel.Value.LazyGeneralSettingsView = new Lazy<object>(() => generalSettingsViewModel.Value.View);
            viewModel.Value.LazyInfoSettingsView = new Lazy<object>(() => infoSettingsViewModel.Value.View);
            viewModel.Value.LazyDeveloperSettingsView = new Lazy<object>(() => developerSettingsViewModel.Value.View);
            return viewModel.Value;
        }

        private GeneralSettingsViewModel InitializeGeneralSettingsViewModel(Lazy<GeneralSettingsViewModel> viewModel)
        {
            viewModel.Value.SelectedAppTheme = DisplayAppThemeExtensions.FromSettings((int?)appDataService.LocalSettings[SettingsKey.Theme]);
            viewModel.Value.FeedManager = FeedManager;
            viewModel.Value.PropertyChanged += GeneralSettingsViewModelPropertyChanged;
            return viewModel.Value;
        }

        private InfoSettingsViewModel InitializeInfoSettingsViewModel(Lazy<InfoSettingsViewModel> viewModel)
        {
            viewModel.Value.LaunchWindowsStoreCommand = launchWindowsStoreCommand;
            viewModel.Value.EnableDeveloperSettingsCommand = enableDeveloperSettingsCommand;
            return viewModel.Value;
        }

        private DeveloperSettingsViewModel InitializeDeveloperSettingsViewModel(Lazy<DeveloperSettingsViewModel> viewModel)
        {
            viewModel.Value.Languages = new[] { "Auto", "en-US", "de-DE" };
            viewModel.Value.SelectedLanguage = (appDataService.LocalSettings[SettingsKey.Language] as string) ?? "Auto";
            viewModel.Value.PropertyChanged += DeveloperSettingsViewModelPropertyChanged;
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
                appDataService.LocalSettings[SettingsKey.Theme] = generalSettingsViewModel.Value.SelectedAppTheme.ToSettings();
            }
        }

        private void DeveloperSettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DeveloperSettingsViewModel.SelectedLanguage))
            {
                if (developerSettingsViewModel.Value.SelectedLanguage == "Auto")
                {
                    appDataService.LocalSettings.Remove(SettingsKey.Language);
                }
                else
                {
                    appDataService.LocalSettings[SettingsKey.Language] = developerSettingsViewModel.Value.SelectedLanguage;
                }
            }
        }
    }
}
