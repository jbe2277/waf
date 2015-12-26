using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Domain;
using System.Waf.UnitTesting;
using System.Waf.Foundation;
using Test.InformationManager.Common.Domain;

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
            var contact = new Contact();

            contact.Email = "jesper.aaberg@example.com";
            Assert.AreEqual("", contact.Validate("Email"));

            contact.Email = "jesper.aaberg@example.";
            Assert.AreEqual("The Email field is not a valid e-mail address.", contact.Validate("Email"));
        }
    }
}
