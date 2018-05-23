using System.Collections.Generic;
using System.Linq;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Test.InformationManager.AddressBook.Modules.Applications.ViewModels
{
    [TestClass]
    public class ContactListViewModelTest : AddressBookTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Container.GetExportedValue<ContactListViewModel>();
            var contacts = new List<Contact>()
            {
                new Contact(),
                new Contact()
            };
            
            Assert.IsNull(viewModel.Contacts);
            viewModel.Contacts = contacts;

            Assert.IsNull(viewModel.SelectedContact);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.SelectedContact, () => viewModel.SelectedContact = contacts.First());
            Assert.AreEqual(contacts.First(), viewModel.SelectedContact);

            Assert.AreEqual("", viewModel.FilterText);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.FilterText, () => viewModel.FilterText = "abc");
            Assert.AreEqual("abc", viewModel.FilterText);
        }

        [TestMethod]
        public void FilterTest()
        {
            var viewModel = Container.GetExportedValue<ContactListViewModel>();
            
            var contact1 = new Contact() { Firstname = "Jesper", Lastname = "Aaberg", Email = "j.a@example.com" };

            Assert.IsTrue(viewModel.Filter(contact1));

            viewModel.FilterText = "jes";
            Assert.IsTrue(viewModel.Filter(contact1));

            viewModel.FilterText = "aab";
            Assert.IsTrue(viewModel.Filter(contact1));

            viewModel.FilterText = "exam";
            Assert.IsTrue(viewModel.Filter(contact1));

            viewModel.FilterText = "wrong filter";
            Assert.IsFalse(viewModel.Filter(contact1));

            // Check that the filter works when the Contact properties are null.
            Assert.IsFalse(viewModel.Filter(new Contact()));
        }

        [TestMethod]
        public void FocusItemTest()
        {
            var viewModel = Container.GetExportedValue<ContactListViewModel>();
            var view = (MockContactListView)viewModel.View;

            bool focusItemActionCalled = false;
            view.FocusItemAction = v =>
            {
                focusItemActionCalled = true;
            };

            viewModel.FocusItem();
            Assert.IsTrue(focusItemActionCalled);
        }
    }
}
