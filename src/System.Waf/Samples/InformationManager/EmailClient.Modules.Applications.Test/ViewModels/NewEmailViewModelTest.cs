using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using System.Waf.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Test.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [TestClass]
    public class NewEmailViewModelTest : EmailClientTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Get<NewEmailViewModel>();

            // Email accounts tests

            var emailAccounts = new List<EmailAccount>()
            {
                new EmailAccount(),
                new EmailAccount()
            };

            AssertHelper.PropertyChangedEvent(viewModel, x => x.SelectedEmailAccount, () => viewModel.SelectedEmailAccount = emailAccounts[0]);
            Assert.AreEqual(emailAccounts[0], viewModel.SelectedEmailAccount);

            // Email tests

            var email = new Email();
            AssertHelper.PropertyChangedEvent(viewModel, x => x.Email, () => viewModel.Email = email);
            Assert.AreEqual(email, viewModel.Email);

            string to = "user@adventure-works.com";
            AssertHelper.PropertyChangedEvent(viewModel, x => x.To, () => viewModel.To = to);
            Assert.AreEqual(to, viewModel.To);
            Assert.AreEqual(to, email.To.Single());

            string cc1 = "harry@example.com";
            string cc2 = "admin@adventure-works.com";
            string cc = cc1 + ", " + cc2;
            AssertHelper.PropertyChangedEvent(viewModel, x => x.CC, () => viewModel.CC = cc);
            Assert.AreEqual(cc1 + "; " + cc2, viewModel.CC);
            AssertHelper.SequenceEqual(new[] { cc1, cc2 }, email.CC);

            string bcc1 = "user@adventure-works.com";
            string bcc2 = "harry@example.com";
            string bcc3 = "admin@adventure-works.com";
            string bcc = bcc1 + "; " + bcc2 + "  " + bcc3;
            AssertHelper.PropertyChangedEvent(viewModel, x => x.Bcc, () => viewModel.Bcc = bcc);
            Assert.AreEqual(bcc1 + "; " + bcc2 + "; " + bcc3, viewModel.Bcc);
            AssertHelper.SequenceEqual(new[] { bcc1, bcc2, bcc3 }, email.Bcc);

            string newEmail = "mike@adventure-works.com";
            AssertHelper.PropertyChangedEvent(viewModel, x => x.To, () => email.To = new[] { newEmail });
            Assert.AreEqual(newEmail, viewModel.To);

            AssertHelper.PropertyChangedEvent(viewModel, x => x.CC, () => email.CC = new[] { newEmail });
            Assert.AreEqual(newEmail, viewModel.CC);

            AssertHelper.PropertyChangedEvent(viewModel, x => x.Bcc, () => email.Bcc = new[] { newEmail });
            Assert.AreEqual(newEmail, viewModel.Bcc);

            viewModel.Email = new Email();
        }

        [TestMethod]
        public void ShowAndCloseTest()
        {
            var viewModel = Get<NewEmailViewModel>();
            var view = (MockNewEmailView)viewModel.View;
            var ownerView = new object();

            viewModel.Show(ownerView);

            Assert.IsTrue(view.IsVisible);
            Assert.AreEqual(ownerView, view.Owner);

            viewModel.CloseCommand.Execute(null);

            Assert.IsFalse(view.IsVisible);
            Assert.IsNull(view.Owner);
        }
    }
}
