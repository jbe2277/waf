using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Domain;
using System.Waf.UnitTesting;
using Test.InformationManager.Common.Domain;

namespace Test.InformationManager.AddressBook.Modules.Domain
{
    [TestClass]
    public class AddressTest : DomainTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var address = new Address();

            AssertHelper.PropertyChangedEvent(address, x => x.Street, () => address.Street = "Main St. 4567");
            Assert.AreEqual("Main St. 4567", address.Street);

            AssertHelper.PropertyChangedEvent(address, x => x.City, () => address.City = "Buffalo");
            Assert.AreEqual("Buffalo", address.City);

            AssertHelper.PropertyChangedEvent(address, x => x.State, () => address.State = "New York");
            Assert.AreEqual("New York", address.State);

            AssertHelper.PropertyChangedEvent(address, x => x.PostalCode, () => address.PostalCode = "98052");
            Assert.AreEqual("98052", address.PostalCode);

            AssertHelper.PropertyChangedEvent(address, x => x.Country, () => address.Country = "United States");
            Assert.AreEqual("United States", address.Country);
        }
    }
}
