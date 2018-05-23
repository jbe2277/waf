using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Applications.SampleData;

namespace Test.InformationManager.AddressBook.Modules.Applications.SampleData
{
    [TestClass]
    public class SampleDataProviderTest
    {
        [TestMethod]
        public void CreateContactsTest()
        {
            var contacts = SampleDataProvider.CreateContacts();
            Assert.IsTrue(contacts.Any());
        }
    }
}
