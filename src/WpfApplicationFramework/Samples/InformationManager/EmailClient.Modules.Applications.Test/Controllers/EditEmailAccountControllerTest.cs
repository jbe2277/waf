using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.Controllers;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Test.InformationManager.EmailClient.Modules.Applications.Views;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using System.Waf.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Test.InformationManager.EmailClient.Modules.Applications.Controllers
{
    [TestClass]
    public class EditEmailAccountControllerTest : EmailClientTest
    {
        [TestMethod]
        public void NewPop3EmailAccount()
        {
            var emailAccount = new EmailAccount();
            EditEmailAccountControllerRun(EditPop3EmailAccountDialog, emailAccount);
        }

        [TestMethod]
        public void EditPop3EmailAccount()
        {
            var emailAccount = new EmailAccount() { EmailAccountSettings = new Pop3Settings() };
            EditEmailAccountControllerRun(EditPop3EmailAccountDialog, emailAccount);
        }

        [TestMethod]
        public void NewExchangeEmailAccount()
        {
            var emailAccount = new EmailAccount();
            EditEmailAccountControllerRun(EditExchangeEmailAccountDialog, emailAccount);
        }

        [TestMethod]
        public void EditExchangeEmailAccount()
        {
            var emailAccount = new EmailAccount() { EmailAccountSettings = new ExchangeSettings() };
            EditEmailAccountControllerRun(EditExchangeEmailAccountDialog, emailAccount);
        }

        private void EditEmailAccountControllerRun(Action<MockEditEmailAccountView> showDialogAction, EmailAccount emailAccount)
        {
            var ownerWindow = new object();
            var controller = Container.GetExportedValue<EditEmailAccountController>();
            controller.OwnerWindow = ownerWindow;
            controller.EmailAccount = emailAccount;
            controller.Initialize();

            bool showDialogCalled = false;
            MockEditEmailAccountView.ShowDialogAction = v =>
            {
                showDialogCalled = true;
                showDialogAction(v);
            };
            controller.Run();
            Assert.IsTrue(showDialogCalled);

            MockEditEmailAccountView.ShowDialogAction = null;
        }

        private void EditPop3EmailAccountDialog(MockEditEmailAccountView editEmailAccountView)
        {
            var editEmailAccountViewModel = ViewHelper.GetViewModel<EditEmailAccountViewModel>(editEmailAccountView);

            // Get the first page of the wizard
            var basicEmailAccountView = (MockBasicEmailAccountView)editEmailAccountViewModel.ContentView;
            var basicEmailAccountViewModel = ViewHelper.GetViewModel<BasicEmailAccountViewModel>(basicEmailAccountView);

            // We are on the first page thus it is not possible to go back
            Assert.IsFalse(editEmailAccountViewModel.BackCommand.CanExecute(null));
            Assert.IsFalse(editEmailAccountViewModel.IsLastPage);

            // Simulate that a validation error occurred. We are not allowed to click on next.
            AssertHelper.CanExecuteChangedEvent(editEmailAccountViewModel.NextCommand, () =>
                editEmailAccountViewModel.IsValid = false);
            Assert.IsFalse(editEmailAccountViewModel.NextCommand.CanExecute(null));

            // Select a Pop3 account and call the next command
            editEmailAccountViewModel.IsValid = true;
            basicEmailAccountViewModel.IsPop3Checked = true;
            editEmailAccountViewModel.NextCommand.Execute(null);

            // We are now on the Pop3 settings page; call the back command
            Assert.IsInstanceOfType(editEmailAccountViewModel.ContentView, typeof(MockPop3SettingsView));
            Assert.IsTrue(editEmailAccountViewModel.IsLastPage);
            AssertHelper.CanExecuteChangedEvent(editEmailAccountViewModel.BackCommand, () => 
                editEmailAccountViewModel.BackCommand.Execute(null));

            // We are back on the first page; call the next command
            Assert.AreEqual(basicEmailAccountView, editEmailAccountViewModel.ContentView);
            Assert.IsFalse(editEmailAccountViewModel.IsLastPage);
            AssertHelper.CanExecuteChangedEvent(editEmailAccountViewModel.BackCommand, () =>
                editEmailAccountViewModel.NextCommand.Execute(null));

            // We are now again on the Pop3 settings page; call the next command
            Assert.IsInstanceOfType(editEmailAccountViewModel.ContentView, typeof(MockPop3SettingsView));
            editEmailAccountViewModel.NextCommand.Execute(null);

            // The wizard is finished
            Assert.IsFalse(editEmailAccountView.IsVisible);
        }

        private void EditExchangeEmailAccountDialog(MockEditEmailAccountView editEmailAccountView)
        {
            var editEmailAccountViewModel = ViewHelper.GetViewModel<EditEmailAccountViewModel>(editEmailAccountView);

            // Get the first page of the wizard
            var basicEmailAccountView = (MockBasicEmailAccountView)editEmailAccountViewModel.ContentView;
            var basicEmailAccountViewModel = ViewHelper.GetViewModel<BasicEmailAccountViewModel>(basicEmailAccountView);

            // Select an Exchange account and call the next command
            basicEmailAccountViewModel.IsExchangeChecked= true;
            editEmailAccountViewModel.NextCommand.Execute(null);

            // We are now on the Exchange settings page
            Assert.IsInstanceOfType(editEmailAccountViewModel.ContentView, typeof(MockExchangeSettingsView));
            Assert.IsTrue(editEmailAccountViewModel.IsLastPage);

            // Go back to the first page and then call the next command twice
            editEmailAccountViewModel.BackCommand.Execute(null);
            Assert.IsFalse(editEmailAccountViewModel.IsLastPage);
            editEmailAccountViewModel.NextCommand.Execute(null);
            editEmailAccountViewModel.NextCommand.Execute(null);

            // The wizard is finished
            Assert.IsFalse(editEmailAccountView.IsVisible);
        }
    }
}
