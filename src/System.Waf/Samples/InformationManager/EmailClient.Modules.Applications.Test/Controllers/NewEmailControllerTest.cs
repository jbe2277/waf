using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Test.InformationManager.AddressBook.Modules.Applications.Services;
using Test.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Interfaces.Domain;
using Waf.InformationManager.EmailClient.Modules.Applications.Controllers;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Test.InformationManager.EmailClient.Modules.Applications.Controllers
{
    [TestClass]
    public class NewEmailControllerTest : EmailClientTest
    {
        [TestMethod]
        public void SendNewEmail()
        {
            var root = new EmailClientRoot();
            var emailAccount = new EmailAccount() { Email = "mike@adventure-works.com" };
            root.AddEmailAccount(emailAccount);

            var controller = Get<NewEmailController>();
            controller.Root = root;
            controller.Initialize();

            // Create a new email

            var newEmailViewModel = controller.NewEmailViewModel;
            var newEmailView = (MockNewEmailView)newEmailViewModel.View;

            controller.Run();

            Assert.IsTrue(newEmailView.IsVisible);
            Assert.AreEqual(emailAccount, newEmailViewModel.SelectedEmailAccount);

            // Select a contact for the To field and cancel the dialog

            var addressBookService = Get<MockAddressBookService>();
            ContactDto? contactResult = null;
            addressBookService.ShowSelectContactViewAction = owner =>
            {
                Assert.AreEqual(newEmailView, owner);
                return contactResult;
            };

            newEmailViewModel.SelectContactCommand.Execute("To");

            // Select a contact for the To field

            contactResult = new ContactDto("", "", "user@adventure-works.com");
            newEmailViewModel.SelectContactCommand.Execute("To");
            Assert.AreEqual("user@adventure-works.com", newEmailViewModel.To);

            // Select a contact for the CC field

            contactResult = new ContactDto("", "", "harry@example.com");
            newEmailViewModel.SelectContactCommand.Execute("CC");
            Assert.AreEqual("harry@example.com", newEmailViewModel.CC);

            // Select a contact for the BCC field

            contactResult = new ContactDto("", "", "admin@adventure-works.com");
            newEmailViewModel.SelectContactCommand.Execute("Bcc");
            Assert.AreEqual("admin@adventure-works.com", newEmailViewModel.Bcc);

            // Pass a wrong parameter to the command => exception
            
            AssertHelper.ExpectedException<ArgumentException>(() => newEmailViewModel.SelectContactCommand.Execute("Wrong field"));

            // Send the email

            newEmailViewModel.SendCommand.Execute(null);

            var sendEmail = root.Sent.Emails.Single();
            Assert.AreEqual("mike@adventure-works.com", sendEmail.From);
            Assert.AreNotEqual(new DateTime(0), sendEmail.Sent);

            Assert.IsFalse(newEmailView.IsVisible);
        }

        [TestMethod]
        public void TrySendNewEmailWithWrongEmailAddresses()
        {
            var root = new EmailClientRoot();
            var emailAccount = new EmailAccount() { Email = "mike@adventure-works.com" };
            root.AddEmailAccount(emailAccount);

            var controller = Get<NewEmailController>();
            controller.Root = root;
            controller.Initialize();

            // Create a new email with a wrong address

            var newEmailViewModel = controller.NewEmailViewModel;
            var newEmailView = (MockNewEmailView)newEmailViewModel.View;

            controller.Run();

            Assert.IsTrue(newEmailView.IsVisible);

            newEmailViewModel.To = "wrong address";

            // Try to send the email => error message occurs

            var messageService = Get<MockMessageService>();
            messageService.Clear();

            newEmailViewModel.SendCommand.Execute(null);

            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsNotNull(messageService.Message);
            Assert.IsFalse(root.Sent.Emails.Any());

            // The view stays open

            Assert.IsTrue(newEmailView.IsVisible);
        }

        [TestMethod]
        public void TryCreateNewEmailWithoutEmailAccounts()
        {
            var root = new EmailClientRoot();

            var controller = Get<NewEmailController>();
            controller.Root = root;
            controller.Initialize();

            // Create a new email but no email account was created => error message

            var messageService = Get<MockMessageService>();
            messageService.Clear();

            controller.Run();

            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsNotNull(messageService.Message);

            var newEmailViewModel = controller.NewEmailViewModel;
            var newEmailView = (MockNewEmailView)newEmailViewModel.View;
            Assert.IsFalse(newEmailView.IsVisible);
        }
    }
}
