using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class NewEmailViewModel : ViewModel<INewEmailView>
    {
        private EmailAccount selectedEmailAccount;
        private Email email;
        private string to = "";
        private string cc = "";
        private string bcc = "";

        [ImportingConstructor]
        public NewEmailViewModel(INewEmailView view) : base(view)
        {
            CloseCommand = new DelegateCommand(Close);
        }

        public ICommand SelectContactCommand { get; set; }

        public ICommand SendCommand { get; set; }

        public ICommand CloseCommand { get; }

        public IReadOnlyList<EmailAccount> EmailAccounts { get; set; }

        public EmailAccount SelectedEmailAccount
        {
            get { return selectedEmailAccount; }
            set { SetProperty(ref selectedEmailAccount, value); }
        }

        public Email Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    if (email != null)
                    {
                        PropertyChangedEventManager.RemoveHandler(email, EmailPropertyChanged, "");
                    }
                    email = value;
                    if (email != null)
                    {
                        PropertyChangedEventManager.AddHandler(email, EmailPropertyChanged, "");
                        UpdateProperties();
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public string To
        {
            get { return to; }
            set
            {
                if (to != value)
                {
                    IReadOnlyList<string> emails = ParseEmails(value);
                    to = FormatEmails(emails);
                    Email.To = emails;
                    RaisePropertyChanged();
                }
            }
        }

        public string CC
        {
            get { return cc; }
            set
            {
                if (cc != value)
                {
                    IReadOnlyList<string> emails = ParseEmails(value);
                    cc = FormatEmails(emails);
                    Email.CC = emails;
                    RaisePropertyChanged();
                }
            }
        }

        public string Bcc
        {
            get { return bcc; }
            set
            {
                if (bcc != value)
                {
                    IReadOnlyList<string> emails = ParseEmails(value);
                    bcc = FormatEmails(emails);
                    Email.Bcc = emails;
                    RaisePropertyChanged();
                }
            }
        }

        public void Show(object owner)
        {
            ViewCore.Show(owner);
        }

        public void Close()
        {
            ViewCore.Close();
        }

        private static IReadOnlyList<string> ParseEmails(string text)
        {
            return text.Trim().Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string FormatEmails(IEnumerable<string> emailList)
        {
            return string.Join("; ", emailList);
        }

        private void UpdateProperties()
        {
            To = FormatEmails(Email.To);
            CC = FormatEmails(Email.CC);
            Bcc = FormatEmails(Email.Bcc);
        }

        private void EmailPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Email.To))
            {
                To = FormatEmails(Email.To);
            }
            else if (e.PropertyName == nameof(CC))
            {
                CC = FormatEmails(Email.CC);
            }
            else if (e.PropertyName == nameof(Bcc))
            {
                Bcc = FormatEmails(Email.Bcc);
            }
        }
    }
}