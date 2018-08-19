using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Domain;
using Test.InformationManager.Common.Domain;

namespace Test.InformationManager.AddressBook.Modules.Domain
{
    [TestClass]
    public class AddressBookRootTest : DomainTest
    {
        [TestMethod]
        public void AddAndRemoveContacts()
        {
            var root = new AddressBookRoot();

            Assert.IsFalse(root.Contacts.Any());
            
            var contact1 = root.AddNewContact();
            CollectionAssert.AreEqual(new[] { contact1 }, root.Contacts.ToArray());

            var contact2 = new Contact();
            root.AddContact(contact2);
            CollectionAssert.AreEqual(new[] { contact1, contact2 }, root.Contacts.ToArray());

            root.RemoveContact(contact1);
            CollectionAssert.AreEqual(new[] { contact2 }, root.Contacts.ToArray());
        }
    }
}
