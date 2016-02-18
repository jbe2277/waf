using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using System.Waf.UnitTesting;
using System.Waf.Applications;
using Test.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [TestClass]
    public class EditEmailAccountViewModelTest : EmailClientTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Container.GetExportedValue<EditEmailAccountViewModel>();

            var emptyCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(viewModel, x => x.BackCommand, () => viewModel.BackCommand = emptyCommand);
            Assert.AreEqual(emptyCommand, viewModel.BackCommand);

            AssertHelper.PropertyChangedEvent(viewModel, x => x.NextCommand, () => viewModel.NextCommand = emptyCommand);
            Assert.AreEqual(emptyCommand, viewModel.NextCommand);

            var contentView = new object();
            AssertHelper.PropertyChangedEvent(viewModel, x => x.ContentView, () => viewModel.ContentView = contentView);
            Assert.AreEqual(contentView, viewModel.ContentView);

            Assert.IsTrue(viewModel.IsValid);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.IsValid, () => viewModel.IsValid = false);
            Assert.IsFalse(viewModel.IsValid);

            Assert.IsFalse(viewModel.IsLastPage);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.IsLastPage, () => viewModel.IsLastPage = true);
            Assert.IsTrue(viewModel.IsLastPage);
        }

        [TestMethod]
        public void ShowDialogAndCloseTest()
        {
            var viewModel = Container.GetExportedValue<EditEmailAccountViewModel>();
            var ownerView = new object();

            bool showDialogCalled = false;
            MockEditEmailAccountView.ShowDialogAction = view =>
            {
                showDialogCalled = true;
                Assert.AreEqual(ownerView, view.Owner);

                Assert.IsTrue(view.IsVisible);
                viewModel.Close();
                Assert.IsFalse(view.IsVisible);
            };
            
            viewModel.ShowDialog(ownerView);

            MockEditEmailAccountView.ShowDialogAction = null;
            Assert.IsTrue(showDialogCalled);
        }
    }
}
