using System.Linq;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.AddressBook.Modules.Applications.Views;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.AddressBook.Modules.Applications.Controllers;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Test.InformationManager.AddressBook.Modules.Applications.Controllers
{
    [TestClass]
    public class ContactControllerTest : AddressBookTest
    {
        [TestMethod]
        public void AddAndRemoveContacts()
        {
            var root = new AddressBookRoot();
            var contact1 = root.AddNewContact();
            
            // Create the controller
            
            var controller = Get<ContactController>();
            var contactLayoutViewModel = Get<ContactLayoutViewModel>();
            var contactListViewModel = controller.ContactListViewModel;
            var contactListView = (MockContactListView)contactListViewModel.View;
            var contactViewModel = controller.ContactViewModel;
            var contactView = (MockContactView)contactViewModel.View;

            // Initialize the controller
            
            Assert.IsNull(contactLayoutViewModel.ContactListView);
            Assert.IsNull(contactLayoutViewModel.ContactView);

            controller.Root = root;
            controller.Initialize();

            Assert.AreEqual(contactListView, contactLayoutViewModel.ContactListView);
            Assert.AreEqual(contactView, contactLayoutViewModel.ContactView);

            // Run the controller

            var shellService = Get<MockShellService>();
            Assert.IsNull(shellService.ContentView);

            controller.Run();

            Assert.AreEqual(contactLayoutViewModel.View, shellService.ContentView);

            // Add a new contact

            bool focusItemCalled = false;
            contactListView.FocusItemAction = view => focusItemCalled = true;
            controller.NewContactCommand.Execute(null);

            Assert.AreEqual(2, root.Contacts.Count);
            var contact2 = root.Contacts[^1];
            Assert.AreEqual(contact2, contactViewModel.Contact);
            Assert.IsTrue(focusItemCalled);

            // Remove the first contact

            contactListViewModel.Contacts = root.Contacts;
            
            AssertHelper.CanExecuteChangedEvent(controller.DeleteContactCommand, () => contactListViewModel.SelectedContact = contact1);
            
            controller.DeleteContactCommand.Execute(null);
            
            Assert.AreEqual(contact2, root.Contacts.Single());
            Assert.AreEqual(contact2, contactListViewModel.SelectedContact);

            // Remove the second contact

            controller.DeleteContactCommand.Execute(null);

            Assert.IsFalse(root.Contacts.Any());
            Assert.IsNull(contactListViewModel.SelectedContact);

            // Check that a delete is not possible because no contacts are left

            Assert.IsFalse(controller.DeleteContactCommand.CanExecute(null));

            // Shutdown the controller

            controller.Shutdown();

            Assert.IsNull(contactLayoutViewModel.ContactListView);
            Assert.IsNull(contactLayoutViewModel.ContactView);
        }
    }
}
