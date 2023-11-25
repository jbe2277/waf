using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views;

[Export(typeof(IPop3SettingsView)), PartCreationPolicy(CreationPolicy.NonShared)]
public partial class Pop3SettingsView : IPop3SettingsView
{
    private readonly Lazy<Pop3SettingsViewModel> viewModel;

    public Pop3SettingsView()
    {
        InitializeComponent();
        viewModel = new(() => this.GetViewModel<Pop3SettingsViewModel>()!);
        Loaded += LoadedHandler;
        Unloaded += UnloadedHandler;
    }

    private Pop3SettingsViewModel ViewModel => viewModel.Value;

    private void LoadedHandler(object sender, RoutedEventArgs e)
    {
        ViewModel.Model.SmtpUserCredits.PropertyChanged += SmtpUserCreditsPropertyChanged;
        serverPathBox.Focus();
    }

    private void UnloadedHandler(object sender, RoutedEventArgs e)
    {
        ViewModel.Model.SmtpUserCredits.PropertyChanged -= SmtpUserCreditsPropertyChanged;
    }

    private void SmtpUserCreditsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(UserCredits.Password)) smtpPassword.Password = ViewModel.Model.SmtpUserCredits.Password;
    }

    private void Pop3PasswordChanged(object sender, RoutedEventArgs e) => ViewModel.Model.Pop3UserCredits.Password = ((PasswordBox)sender).Password;

    private void SmtpPasswordChanged(object sender, RoutedEventArgs e) => ViewModel.Model.SmtpUserCredits.Password = ((PasswordBox)sender).Password;
}
