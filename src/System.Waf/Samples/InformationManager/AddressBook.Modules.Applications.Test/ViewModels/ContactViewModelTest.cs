using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Test.InformationManager.AddressBook.Modules.Applications.ViewModels;

[TestClass]
public class ContactViewModelTest : AddressBookTest
{
    [TestMethod]
    public void PropertiesTest()
    {
        var viewModel = Get<ContactViewModel>();

        Assert.IsNull(viewModel.Contact);

        var contact = new Contact();
        AssertHelper.PropertyChangedEvent(viewModel, x => x.Contact, () => viewModel.Contact = contact);
        Assert.AreEqual(contact, viewModel.Contact);
    }
}
