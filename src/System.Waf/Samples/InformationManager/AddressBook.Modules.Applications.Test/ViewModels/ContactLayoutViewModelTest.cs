using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

namespace Test.InformationManager.AddressBook.Modules.Applications.ViewModels
{
    [TestClass]
    public class ContactLayoutViewModelTest : AddressBookTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Get<ContactLayoutViewModel>();

            Assert.IsNull(viewModel.ContactListView);
            var contactListView = new object();
            AssertHelper.PropertyChangedEvent(viewModel, x => x.ContactListView, () => viewModel.ContactListView = contactListView);
            Assert.AreEqual(contactListView, viewModel.ContactListView);

            Assert.IsNull(viewModel.ContactView);
            var contactView = new object();
            AssertHelper.PropertyChangedEvent(viewModel, x => x.ContactView, () => viewModel.ContactView = contactView);
            Assert.AreEqual(contactView, viewModel.ContactView);
        }
    }
}
