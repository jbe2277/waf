using System.Waf.Applications;
using System.Waf.Applications.Services;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers;

internal sealed class SettingsController
{
    private const string autoLanguage = "Auto";
    private static readonly IReadOnlyList<string> languages = [autoLanguage, "en-US", "de-DE"];
    
    private readonly AppSettings appSettings;
    private readonly DataController dataController;
    private readonly Lazy<SettingsViewModel> settingsViewModel;
    private readonly DelegateCommand enableDeveloperSettingsCommand;
    private int enableDeveloperSettingsCallCount;

    public SettingsController(ISettingsService settingsService, DataController dataController, Lazy<SettingsViewModel> settingsViewModel)
    {
        appSettings = settingsService.Get<AppSettings>();
        this.dataController = dataController;
        this.settingsViewModel = new(() => InitializeViewModel(settingsViewModel.Value));
        enableDeveloperSettingsCommand = new(() =>
        {
            enableDeveloperSettingsCallCount++;
            if (enableDeveloperSettingsCallCount > 2) settingsViewModel.Value.DeveloperSettingsEnabled = true;
        });
    }

    public FeedManager FeedManager { get; set; } = null!;

    public IViewModelCore SettingsViewModel => settingsViewModel.Value;

    private SettingsViewModel InitializeViewModel(SettingsViewModel vm)
    {
        vm.FeedManager = FeedManager;
        vm.EnableDeveloperSettingsCommand = enableDeveloperSettingsCommand;
        vm.SignInCommand = dataController.SignInCommand;
        vm.SignOutCommand = dataController.SignOutCommand;
        vm.Languages = languages;
        vm.SelectedLanguage = appSettings.Language ?? autoLanguage;
        vm.PropertyChanged += SettingsViewModelPropertyChanged;
        vm.Initialize();
        return vm;
    }

    private void SettingsViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ViewModels.SettingsViewModel.SelectedLanguage))
        {
            appSettings.Language = settingsViewModel.Value.SelectedLanguage == autoLanguage ? null : settingsViewModel.Value.SelectedLanguage;
        }
    }
}
