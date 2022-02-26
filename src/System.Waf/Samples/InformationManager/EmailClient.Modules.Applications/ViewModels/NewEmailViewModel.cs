using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class NewEmailViewModel : ViewModel<INewEmailView>
    {
        private EmailAccount? selectedEmailAccount;
        private Email email = null!;
        private string to = "";
        private string cc = "";
        private string bcc = "";
        private IWeakEventProxy? emailPropertyChangedProxy;

        [ImportingConstructor]
        public NewEmailViewModel(INewEmailView view) : base(view)
        {
            CloseCommand = new DelegateCommand(Close);
        }

        public ICommand SelectContactCommand { get; set; } = DelegateCommand.DisabledCommand;

        public ICommand SendCommand { get; set; } = DelegateCommand.DisabledCommand;

        public ICommand CloseCommand { get; }

        public IReadOnlyList<EmailAccount> EmailAccounts { get; set; } = null!;

        public EmailAccount? SelectedEmailAccount
        {
            get => selectedEmailAccount;
            set => SetProperty(ref selectedEmailAccount, value);
        }

        public Email Email
        {
            get => email;
            set
            {
                if (email == value) return;
                emailPropertyChangedProxy?.Remove();
                email = value;
                emailPropertyChangedProxy = WeakEvent.PropertyChanged.Add(email, EmailPropertyChanged);
                UpdateProperties();
                RaisePropertyChanged();
            }
        }

        public string To
        {
            get => to;
            set
            {
                if (to == value) return;
                var emails = ParseEmails(value);
                to = FormatEmails(emails);
                Email.To = emails;
                RaisePropertyChanged();
            }
        }

        public string CC
        {
            get => cc;
            set
            {
                if (cc == value) return;
                var emails = ParseEmails(value);
                cc = FormatEmails(emails);
                Email.CC = emails;
                RaisePropertyChanged();
            }
        }

        public string Bcc
        {
            get => bcc;
            set
            {
                if (bcc == value) return;
                var emails = ParseEmails(value);
                bcc = FormatEmails(emails);
                Email.Bcc = emails;
                RaisePropertyChanged();
            }
        }

        public void Show(object owner) => ViewCore.Show(owner);

        public void Close() => ViewCore.Close();

        private static IReadOnlyList<string> ParseEmails(string text) => text.Trim().Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        private static string FormatEmails(IEnumerable<string> emailList) => string.Join("; ", emailList);

        private void UpdateProperties()
        {
            To = FormatEmails(Email.To);
            CC = FormatEmails(Email.CC);
            Bcc = FormatEmails(Email.Bcc);
        }

        private void EmailPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Email.To)) To = FormatEmails(Email.To);
            else if (e.PropertyName == nameof(CC)) CC = FormatEmails(Email.CC);
            else if (e.PropertyName == nameof(Bcc)) Bcc = FormatEmails(Email.Bcc);
        }
    }
}