using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Domain;
using Test.InformationManager.Common.Domain;
using System.Waf.UnitTesting;

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
            AssertHelper.SequenceEqual(new[] { contact1 }, root.Contacts);

            var contact2 = new Contact();
            root.AddContact(contact2);
            AssertHelper.SequenceEqual(new[] { contact1, contact2 }, root.Contacts);

            root.RemoveContact(contact1);
            AssertHelper.SequenceEqual(new[] { contact2 }, root.Contacts);
        }
    }
}
