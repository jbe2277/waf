using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Test.InformationManager.Common.Domain;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Test.InformationManager.AddressBook.Modules.Domain
{
    [TestClass]
    public class ContactTest : DomainTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var contact = new Contact();

            AssertHelper.PropertyChangedEvent(contact, x => x.Firstname, () => contact.Firstname = "Jesper");
            Assert.AreEqual("Jesper", contact.Firstname);

            AssertHelper.PropertyChangedEvent(contact, x => x.Lastname, () => contact.Lastname = "Aaberg");
            Assert.AreEqual("Aaberg", contact.Lastname);

            AssertHelper.PropertyChangedEvent(contact, x => x.Company, () => contact.Company = "A. Datum Corporation");
            Assert.AreEqual("A. Datum Corporation", contact.Company);

            AssertHelper.PropertyChangedEvent(contact, x => x.Email, () => contact.Email = "jesper.aaberg@example.com");
            Assert.AreEqual("jesper.aaberg@example.com", contact.Email);

            AssertHelper.PropertyChangedEvent(contact, x => x.Phone, () => contact.Phone = "(111) 555-0100");
            Assert.AreEqual("(111) 555-0100", contact.Phone);

            contact.Address.Country = "United States";
            Assert.AreEqual("United States", contact.Address.Country);
        }

        [TestMethod]
        public void ValidationTest()
        {
            var root = new AddressBookRoot();
            var contact = root.AddNewContact();
            Assert.AreEqual(nameof(contact.Firstname), contact.Errors.Single().MemberNames.Single());
            contact.Firstname = "Jesper";
            Assert.AreEqual(0, contact.Errors.Count);

            contact.Email = "jesper.aaberg@example.com";
            Assert.IsFalse(contact.HasErrors);

            contact.Email = "jesper.aaberg@";
            Assert.AreEqual("The Email field is not a valid e-mail address.", contact.Errors.Single().ErrorMessage);
        }
    }
}
