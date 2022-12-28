using System;
using System.ComponentModel;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers;

internal class SettingsController
{
    private readonly AppSettings appSettings;
    private readonly DataController dataController;
    private readonly Lazy<SettingsViewModel> settingsViewModel;
    private readonly DelegateCommand enableDeveloperSettingsCommand;
    private int enableDeveloperSettingsCallCount;

    public SettingsController(ISettingsService settingsService, DataController dataController, Lazy<SettingsViewModel> settingsViewModel)
    {
        appSettings = settingsService.Get<AppSettings>();
        this.dataController = dataController;
        this.settingsViewModel = new Lazy<SettingsViewModel>(() => InitializeViewModel(settingsViewModel.Value));
        enableDeveloperSettingsCommand = new DelegateCommand(() =>
        {
            enableDeveloperSettingsCallCount++;
            if (enableDeveloperSettingsCallCount > 2) settingsViewModel.Value.DeveloperSettingsEnabled = true;
        });
    }

    public FeedManager FeedManager { get; set; } = null!;

    public IViewModelCore SettingsViewModel => settingsViewModel.Value;

    private SettingsViewModel InitializeViewModel(SettingsViewModel viewModel)
    {
        viewModel.FeedManager = FeedManager;
        viewModel.EnableDeveloperSettingsCommand = enableDeveloperSettingsCommand;
        viewModel.SignInCommand = dataController.SignInCommand;
        viewModel.SignOutCommand = dataController.SignOutCommand;
        viewModel.Languages = new[] { "Auto", "en-US", "de-DE" };
        viewModel.SelectedLanguage = appSettings.Language ?? "Auto";
        viewModel.PropertyChanged += DeveloperSettingsViewModelPropertyChanged;
        viewModel.Initialize();
        return viewModel;
    }

    private void DeveloperSettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModels.SettingsViewModel.SelectedLanguage))
        {
            appSettings.Language = settingsViewModel.Value.SelectedLanguage == "Auto" ? null : settingsViewModel.Value.SelectedLanguage;
        }
    }
}
