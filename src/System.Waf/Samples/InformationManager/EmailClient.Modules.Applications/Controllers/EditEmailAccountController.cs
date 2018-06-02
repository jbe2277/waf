using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Controllers
{
    /// <summary>
    /// Responsible for creating a new email account or modifying an existing one via a wizard.
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class EditEmailAccountController
    {
        private readonly DelegateCommand backCommand;
        private readonly DelegateCommand nextCommand;
        private readonly EditEmailAccountViewModel editEmailAccountViewModel;
        private readonly BasicEmailAccountViewModel basicEmailAccountViewModel;
        private readonly ExportFactory<Pop3SettingsViewModel> pop3SettingsViewModelFactory;
        private readonly ExportFactory<ExchangeSettingsViewModel> exchangeSettingsViewModelFactory;
        private Pop3SettingsViewModel pop3SettingsViewModel;
        private ExchangeSettingsViewModel exchangeSettingsViewModel;
        private bool result;

        [ImportingConstructor]
        public EditEmailAccountController(EditEmailAccountViewModel editEmailAccountViewModel,
            BasicEmailAccountViewModel basicEmailAccountViewModel, ExportFactory<Pop3SettingsViewModel> pop3SettingsViewModelFactory, 
            ExportFactory<ExchangeSettingsViewModel> exchangeSettingsViewModelFactory)
        {
            this.editEmailAccountViewModel = editEmailAccountViewModel;
            this.basicEmailAccountViewModel = basicEmailAccountViewModel;
            this.pop3SettingsViewModelFactory = pop3SettingsViewModelFactory;
            this.exchangeSettingsViewModelFactory = exchangeSettingsViewModelFactory;
            backCommand = new DelegateCommand(Back, CanBack);
            nextCommand = new DelegateCommand(Next, CanNext);
        }

        public object OwnerWindow { get; set; }

        public EmailAccount EmailAccount { get; set; }
        
        public void Initialize()
        {
            PropertyChangedEventManager.AddHandler(editEmailAccountViewModel, EditEmailAccountViewModelPropertyChanged, "");
            editEmailAccountViewModel.BackCommand = backCommand;
            editEmailAccountViewModel.NextCommand = nextCommand;
            basicEmailAccountViewModel.EmailAccount = EmailAccount;
        }

        public bool Run()
        {
            editEmailAccountViewModel.ContentView = basicEmailAccountViewModel.View;
            editEmailAccountViewModel.ShowDialog(OwnerWindow);
            return result;
        }

        private void Close()
        {
            editEmailAccountViewModel.Close();
        }

        private bool CanBack()
        {
            return editEmailAccountViewModel.ContentView != basicEmailAccountViewModel.View;
        }

        private void Back()
        {
            editEmailAccountViewModel.IsLastPage = false;
            editEmailAccountViewModel.ContentView = basicEmailAccountViewModel.View;
            UpdateCommandsState();
        }

        private bool CanNext() { return editEmailAccountViewModel.IsValid; }

        private void Next()
        {
            if (editEmailAccountViewModel.ContentView == basicEmailAccountViewModel.View)
            {
                if (basicEmailAccountViewModel.IsPop3Checked)
                {
                    editEmailAccountViewModel.IsLastPage = true;
                    ShowPop3SettingsView();
                }
                else if (basicEmailAccountViewModel.IsExchangeChecked)
                {
                    editEmailAccountViewModel.IsLastPage = true;
                    ShowExchangeSettingsView();
                }
            }
            else if (pop3SettingsViewModel != null && editEmailAccountViewModel.ContentView == pop3SettingsViewModel.View)
            {
                SavePop3Settings();
                Close();
            }
            else if (exchangeSettingsViewModel != null && editEmailAccountViewModel.ContentView == exchangeSettingsViewModel.View)
            {
                SaveExchangeSettings();
                Close();
            }
            UpdateCommandsState();
        }

        private void UpdateCommandsState()
        {
            backCommand.RaiseCanExecuteChanged();
            nextCommand.RaiseCanExecuteChanged();
        }
        
        private void EditEmailAccountViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EditEmailAccountViewModel.IsValid))
            {
                UpdateCommandsState();
            }
        }
        
        private void ShowPop3SettingsView()
        {
            Pop3Settings pop3Settings = EmailAccount.EmailAccountSettings is Pop3Settings 
                ? (Pop3Settings)EmailAccount.EmailAccountSettings : new Pop3Settings();
            pop3Settings.Validate();
            pop3SettingsViewModel = pop3SettingsViewModelFactory.CreateExport().Value;
            pop3SettingsViewModel.Model = pop3Settings;
            editEmailAccountViewModel.ContentView = pop3SettingsViewModel.View;
        }

        private void ShowExchangeSettingsView()
        {
            ExchangeSettings exchangeSettings = EmailAccount.EmailAccountSettings is ExchangeSettings 
                ? (ExchangeSettings)EmailAccount.EmailAccountSettings : new ExchangeSettings();
            exchangeSettings.Validate();
            exchangeSettingsViewModel = exchangeSettingsViewModelFactory.CreateExport().Value;
            exchangeSettingsViewModel.Model = exchangeSettings;
            editEmailAccountViewModel.ContentView = exchangeSettingsViewModel.View;
        }

        private void SavePop3Settings()
        {
            EmailAccount.EmailAccountSettings = pop3SettingsViewModel.Model;
            result = true;
        }

        private void SaveExchangeSettings()
        {
            EmailAccount.EmailAccountSettings = exchangeSettingsViewModel.Model;
            result = true;
        }
    }
}
