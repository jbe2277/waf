using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.Controllers;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Test.InformationManager.EmailClient.Modules.Applications.Views;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using System.Waf.UnitTesting;

namespace Test.InformationManager.EmailClient.Modules.Applications.Controllers
{
    [TestClass]
    public class EmailAccountsControllerTest : EmailClientTest
    {
        protected override void OnCleanup()
        {
            MockEmailAccountsView.ShowDialogAction = null;
            base.OnCleanup();
        }

        [TestMethod]
        public void NewEditAndRemoveEmailAccount()
        {
            var root = new EmailClientRoot();
            var controller = Get<EmailAccountsController>();
            controller.Root = root;

            bool showDialogCalled = false;
            MockEmailAccountsView.ShowDialogAction = v =>
            {
                showDialogCalled = true;
                EmailAccountsViewShowDialog(v);
            };

            controller.EmailAccountsCommand.Execute(null);
            Assert.IsTrue(showDialogCalled);
        }

        private static void EmailAccountsViewShowDialog(MockEmailAccountsView view)
        {
            var viewModel = ViewHelper.GetViewModel<EmailAccountsViewModel>(view)!;
            Assert.IsFalse(viewModel.EmailClientRoot.EmailAccounts.Any());
            Assert.IsNull(viewModel.SelectedEmailAccount);
            Assert.IsFalse(viewModel.EditAccountCommand.CanExecute(null));
            Assert.IsFalse(viewModel.RemoveAccountCommand.CanExecute(null));

            // Create a new email account

            bool showDialogCalled = false;
            MockEditEmailAccountView.ShowDialogAction = v =>
            {
                showDialogCalled = true;
                EditEmailAccountViewShowDialog(v);
            };

            viewModel.NewAccountCommand.Execute(null);
            Assert.IsTrue(showDialogCalled);
            MockEditEmailAccountView.ShowDialogAction = null;

            var emailAccount = viewModel.EmailClientRoot.EmailAccounts.Single();
            AssertHelper.CanExecuteChangedEvent(viewModel.RemoveAccountCommand, () => viewModel.SelectedEmailAccount = emailAccount);
            Assert.IsTrue(viewModel.EditAccountCommand.CanExecute(null));
            Assert.IsTrue(viewModel.RemoveAccountCommand.CanExecute(null));

            // Edit the email account

            showDialogCalled = false;
            MockEditEmailAccountView.ShowDialogAction = v =>
            {
                showDialogCalled = true;
                EditEmailAccountViewShowDialog(v);
            };

            viewModel.EditAccountCommand.Execute(null);
            Assert.IsTrue(showDialogCalled);
            MockEditEmailAccountView.ShowDialogAction = null;

            var editedEmailAccount = viewModel.EmailClientRoot.EmailAccounts.Single();
            Assert.AreNotEqual(emailAccount, editedEmailAccount);
            AssertHelper.CanExecuteChangedEvent(viewModel.EditAccountCommand, () => viewModel.SelectedEmailAccount = editedEmailAccount);

            // Remove the email account

            viewModel.RemoveAccountCommand.Execute(null);
            Assert.IsFalse(viewModel.EmailClientRoot.EmailAccounts.Any());
        }

        private static void EditEmailAccountViewShowDialog(MockEditEmailAccountView view)
        {
            var viewModel = ViewHelper.GetViewModel<EditEmailAccountViewModel>(view)!;
            
            // Just finish the wizard with default settings
            viewModel.NextCommand.Execute(null);
            viewModel.NextCommand.Execute(null);
        }
    }
}
