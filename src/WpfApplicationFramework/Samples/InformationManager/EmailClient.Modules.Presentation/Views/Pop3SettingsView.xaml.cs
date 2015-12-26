using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views
{
    [Export(typeof(IPop3SettingsView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Pop3SettingsView : UserControl, IPop3SettingsView
    {
        private readonly Lazy<Pop3SettingsViewModel> viewModel;


        public Pop3SettingsView()
        {
            InitializeComponent();

            viewModel = new Lazy<Pop3SettingsViewModel>(() => ViewHelper.GetViewModel<Pop3SettingsViewModel>(this));
            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }


        private Pop3SettingsViewModel ViewModel { get { return viewModel.Value; } }


        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Model.SmtpUserCredits.PropertyChanged += SmtpUserCreditsPropertyChanged;
            serverPathBox.Focus();
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Model.SmtpUserCredits.PropertyChanged -= SmtpUserCreditsPropertyChanged;
        }

        private void SmtpUserCreditsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Password")
            {
                smtpPassword.Password = ViewModel.Model.SmtpUserCredits.Password;
            }
        }

        private void Pop3PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            ViewModel.Model.Pop3UserCredits.Password = passwordBox.Password;
        }

        private void SmtpPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            ViewModel.Model.SmtpUserCredits.Password = passwordBox.Password;
        }
    }
}
