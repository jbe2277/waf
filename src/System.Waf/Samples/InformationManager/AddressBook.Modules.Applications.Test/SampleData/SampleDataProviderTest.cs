using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Applications.SampleData;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Test.InformationManager.AddressBook.Modules.Applications.SampleData
{
    [TestClass]
    public class SampleDataProviderTest
    {
        [TestMethod]
        public void CreateContactsTest()
        {
            IReadOnlyList<Contact> contacts = SampleDataProvider.CreateContacts();
            Assert.IsTrue(contacts.Any());
        }
    }
}
