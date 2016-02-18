using System.Linq;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class EmailAccountsViewModel : ViewModel<IEmailAccountsView>
    {
        private EmailClientRoot emailClientRoot;
        private ICommand newAccountCommand;
        private ICommand removeAccountCommand;
        private ICommand editAccountCommand;
        private EmailAccount selectedEmailAccount;

        
        [ImportingConstructor]
        public EmailAccountsViewModel(IEmailAccountsView view) : base(view)
        {
        }


        public EmailClientRoot EmailClientRoot
        {
            get { return emailClientRoot; }
            set
            {
                if (emailClientRoot != value)
                {
                    emailClientRoot = value;
                    if (emailClientRoot != null)
                    {
                        SelectedEmailAccount = emailClientRoot.EmailAccounts.FirstOrDefault();
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand NewAccountCommand
        {
            get { return newAccountCommand; }
            set { SetProperty(ref newAccountCommand, value); }
        }

        public ICommand RemoveAccountCommand
        {
            get { return removeAccountCommand; }
            set { SetProperty(ref removeAccountCommand, value); }
        }

        public ICommand EditAccountCommand
        {
            get { return editAccountCommand; }
            set { SetProperty(ref editAccountCommand, value); }
        }

        public EmailAccount SelectedEmailAccount
        {
            get { return selectedEmailAccount; }
            set { SetProperty(ref selectedEmailAccount, value); }
        }


        public void ShowDialog(object owner)
        {
            ViewCore.ShowDialog(owner);
        }
    }
}
