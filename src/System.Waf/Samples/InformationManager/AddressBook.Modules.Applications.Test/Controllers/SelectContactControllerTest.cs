using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Applications.Controllers;
using Waf.InformationManager.AddressBook.Modules.Domain;
using Test.InformationManager.AddressBook.Modules.Applications.Views;
using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using System.Waf.UnitTesting;

namespace Test.InformationManager.AddressBook.Modules.Applications.Controllers
{
    [TestClass]
    public class SelectContactControllerTest : AddressBookTest
    {
        protected override void OnCleanup()
        {
            MockSelectContactView.ShowDialogAction = null;
            base.OnCleanup();
        }

        [TestMethod]
        public void SelectContactTest()
        {
            var root = new AddressBookRoot();
            var contact1 = root.AddNewContact();
            var contact2 = root.AddNewContact();
            
            var controller = Get<SelectContactController>();

            controller.OwnerView = new object();
            controller.Root = root;

            controller.Initialize();

            MockSelectContactView.ShowDialogAction = view =>
            {
                var vm = ViewHelper.GetViewModel<SelectContactViewModel>(view)!;
                Assert.AreEqual(contact1, controller.ContactListViewModel.SelectedContact);

                AssertHelper.CanExecuteChangedEvent(vm.OkCommand, () => controller.ContactListViewModel.SelectedContact = null);
                Assert.IsFalse(vm.OkCommand.CanExecute(null));
                
                AssertHelper.CanExecuteChangedEvent(vm.OkCommand, () => controller.ContactListViewModel.SelectedContact = contact2);
                Assert.IsTrue(vm.OkCommand.CanExecute(null));
                
                vm.OkCommand.Execute(null);

                Assert.IsFalse(view.IsVisible);
            };

            controller.Run();
            Assert.AreEqual(contact2, controller.SelectedContact);
        }

        [TestMethod]
        public void SelectContactAndCancelTest()
        {
            var root = new AddressBookRoot();
            root.AddNewContact();
            root.AddNewContact();

            var controller = Get<SelectContactController>();

            controller.OwnerView = new object();
            controller.Root = root;

            controller.Initialize();

            bool showDialogActionCalled = false;
            MockSelectContactView.ShowDialogAction = _ =>
            {
                showDialogActionCalled = true;
                // Do nothing, this simulates that the user has closed the window.
            };

            controller.Run();
            Assert.IsTrue(showDialogActionCalled);
            Assert.IsNull(controller.SelectedContact);
        }
    }
}
