using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using Waf.InformationManager.AddressBook.Interfaces.Applications;
using Waf.InformationManager.AddressBook.Interfaces.Domain;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Controllers
{
    /// <summary>
    /// Responsible for creating a new email and sending it.
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class NewEmailController
    {
        private readonly IMessageService messageService;
        private readonly IShellService shellService;
        private readonly IAddressBookService addressBookService;
        private readonly DelegateCommand selectContactCommand;
        private readonly DelegateCommand sendCommand;
        
        [ImportingConstructor]
        public NewEmailController(IMessageService messageService, IShellService shellService, IAddressBookService addressBookService, NewEmailViewModel newEmailViewModel)
        {
            this.messageService = messageService;
            this.shellService = shellService;
            this.addressBookService = addressBookService;
            NewEmailViewModel = newEmailViewModel;
            selectContactCommand = new DelegateCommand(SelectContact);
            sendCommand = new DelegateCommand(SendEmail);
        }

        public EmailClientRoot Root { get; set; }

        internal NewEmailViewModel NewEmailViewModel { get; }

        public void Initialize()
        {
            NewEmailViewModel.SelectContactCommand = selectContactCommand;
            NewEmailViewModel.SendCommand = sendCommand;
            NewEmailViewModel.EmailAccounts = Root.EmailAccounts;
            NewEmailViewModel.SelectedEmailAccount = Root.EmailAccounts.FirstOrDefault();
            var newEmail = new Email() { EmailType = EmailType.Sent };
            newEmail.Validate();
            NewEmailViewModel.Email = newEmail;
        }

        public void Run()
        {
            if (!Root.EmailAccounts.Any())
            {
                messageService.ShowError("Please create an email account first.");
                return;
            }
            NewEmailViewModel.Show(shellService.ShellView);
        }

        private void SelectContact(object commandParameter)
        {
            ContactDto selectedContact = addressBookService.ShowSelectContactView(NewEmailViewModel.View);
            if (selectedContact == null) { return; }

            string parameter = commandParameter as string;
            if (parameter == "To")
            {
                NewEmailViewModel.To += " " + selectedContact.Email;
            }
            else if (parameter == "CC")
            {
                NewEmailViewModel.CC += " " + selectedContact.Email;
            }
            else if (parameter == "Bcc")
            {
                NewEmailViewModel.Bcc += " " + selectedContact.Email;
            }
            else
            {
                throw new ArgumentException("Invalid commandParameter value");
            }
        }

        private void SendEmail()
        {
            var email = NewEmailViewModel.Email;
            if (email.HasErrors)
            {
                messageService.ShowError("One or more fields are not valid. Please correct them before sending this email.\n\n" 
                    + string.Join("\n", email.Errors.Select(x => x.ErrorMessage)));
                return;
            }
            NewEmailViewModel.Close();
            email.From = NewEmailViewModel.SelectedEmailAccount.Email;
            email.Sent = DateTime.Now;
            Root.Sent.AddEmail(email);
        }
    }
}
