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
        private EmailAccount selectedEmailAccount;

        [ImportingConstructor]
        public EmailAccountsViewModel(IEmailAccountsView view) : base(view)
        {
        }

        public EmailClientRoot EmailClientRoot { get; set; }

        public ICommand NewAccountCommand { get; set; }

        public ICommand RemoveAccountCommand { get; set; }

        public ICommand EditAccountCommand { get; set; }

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
