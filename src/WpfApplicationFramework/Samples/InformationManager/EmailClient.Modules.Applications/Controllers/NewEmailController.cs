using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.Foundation;
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
        private readonly NewEmailViewModel newEmailViewModel;
        private readonly DelegateCommand selectContactCommand;
        private readonly DelegateCommand sendCommand;
        

        [ImportingConstructor]
        public NewEmailController(IMessageService messageService, IShellService shellService, IAddressBookService addressBookService, NewEmailViewModel newEmailViewModel)
        {
            this.messageService = messageService;
            this.shellService = shellService;
            this.addressBookService = addressBookService;
            this.newEmailViewModel = newEmailViewModel;
            this.selectContactCommand = new DelegateCommand(SelectContact);
            this.sendCommand = new DelegateCommand(SendEmail);
        }


        public EmailClientRoot Root { get; set; }

        internal NewEmailViewModel NewEmailViewModel { get { return newEmailViewModel; } }


        public void Initialize()
        {
            newEmailViewModel.SelectContactCommand = selectContactCommand;
            newEmailViewModel.SendCommand = sendCommand;
            newEmailViewModel.EmailAccounts = Root.EmailAccounts;
            newEmailViewModel.SelectedEmailAccount = Root.EmailAccounts.FirstOrDefault();
            newEmailViewModel.Email = new Email() { EmailType = EmailType.Sent };
        }

        public void Run()
        {
            if (!Root.EmailAccounts.Any())
            {
                messageService.ShowError("Please create an email account first.");
                return;
            }

            newEmailViewModel.Show(shellService.ShellView);
        }

        private void SelectContact(object commandParameter)
        {
            ContactDto selectedContact = addressBookService.ShowSelectContactView(newEmailViewModel.View);
            if (selectedContact == null) { return; }

            string parameter = commandParameter as string;
            if (parameter == "To")
            {
                newEmailViewModel.To += " " + selectedContact.Email;
            }
            else if (parameter == "CC")
            {
                newEmailViewModel.CC += " " + selectedContact.Email;
            }
            else if (parameter == "Bcc")
            {
                newEmailViewModel.Bcc += " " + selectedContact.Email;
            }
            else
            {
                throw new ArgumentException("Invalid commandParameter value");
            }
        }

        private void SendEmail()
        {
            var email = newEmailViewModel.Email;
            string errorMessage = email.Validate();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                messageService.ShowError("One or more fields are not valid. Please correct them before sending this email.\n\n" + errorMessage);
                return;
            }

            newEmailViewModel.Close();
            
            email.From = newEmailViewModel.SelectedEmailAccount.Email;
            email.Sent = DateTime.Now;
            Root.Sent.AddEmail(email);
        }
    }
}
