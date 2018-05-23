using System.Waf.Applications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

namespace Test.InformationManager.AddressBook.Modules.Applications.ViewModels
{
    [TestClass]
    public class SelectContactViewModelTest : AddressBookTest
    {
        [TestMethod]
        public void ShowDialogAndCloseTest()
        {
            var viewModel = Container.GetExportedValue<SelectContactViewModel>();

            bool showDialogActionCalled = false;
            MockSelectContactView.ShowDialogAction = view =>
            {
                showDialogActionCalled = true;
                var vm = ViewHelper.GetViewModel<SelectContactViewModel>(view);
                vm.Close();
                Assert.IsFalse(view.IsVisible);
            };

            object owner = new object();
            viewModel.ShowDialog(owner);
            Assert.IsTrue(showDialogActionCalled);

            MockSelectContactView.ShowDialogAction = null;
        }
    }
}
