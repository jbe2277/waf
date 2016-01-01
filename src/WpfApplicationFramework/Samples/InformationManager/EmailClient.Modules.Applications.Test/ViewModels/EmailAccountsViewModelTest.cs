using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using System.Waf.UnitTesting;
using System.Waf.Applications;
using Test.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [TestClass]
    public class EmailAccountsViewModelTest : EmailClientTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Container.GetExportedValue<EmailAccountsViewModel>();

            var root = new EmailClientRoot();
            AssertHelper.PropertyChangedEvent(viewModel, x => x.EmailClientRoot, () => viewModel.EmailClientRoot = root);
            Assert.AreEqual(root, viewModel.EmailClientRoot);

            root = new EmailClientRoot();
            root.AddEmailAccount(new EmailAccount());
            root.AddEmailAccount(new EmailAccount());
            AssertHelper.PropertyChangedEvent(viewModel, x => x.SelectedEmailAccount, () => viewModel.EmailClientRoot = root);
            Assert.AreEqual(root.EmailAccounts.First(), viewModel.SelectedEmailAccount);

            AssertHelper.PropertyChangedEvent(viewModel, x => x.SelectedEmailAccount, () => viewModel.SelectedEmailAccount = root.EmailAccounts.ElementAt(1));
            Assert.AreEqual(root.EmailAccounts.ElementAt(1), viewModel.SelectedEmailAccount);

            var emptyCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(viewModel, x => x.NewAccountCommand, () => viewModel.NewAccountCommand = emptyCommand);
            Assert.AreEqual(emptyCommand, viewModel.NewAccountCommand);

            AssertHelper.PropertyChangedEvent(viewModel, x => x.RemoveAccountCommand, () => viewModel.RemoveAccountCommand = emptyCommand);
            Assert.AreEqual(emptyCommand, viewModel.RemoveAccountCommand);

            AssertHelper.PropertyChangedEvent(viewModel, x => x.EditAccountCommand, () => viewModel.EditAccountCommand = emptyCommand);
            Assert.AreEqual(emptyCommand, viewModel.EditAccountCommand);
        }

        [TestMethod]
        public void ShowDialogTest()
        {
            var viewModel = Container.GetExportedValue<EmailAccountsViewModel>();
            var ownerView = new object();

            bool showDialogCalled = false;
            MockEmailAccountsView.ShowDialogAction = view =>
            {
                showDialogCalled = true;
                Assert.AreEqual(ownerView, view.Owner);
            };

            viewModel.ShowDialog(ownerView);

            MockEmailAccountsView.ShowDialogAction = null;
            Assert.IsTrue(showDialogCalled);
        }
    }
}
