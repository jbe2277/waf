using System.Collections.Generic;
using Waf.InformationManager.Common.Domain;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Waf.InformationManager.AddressBook.Modules.Domain
{
    [DataContract]
    public class AddressBookRoot : ValidationModel
    {
        [DataMember]
        private readonly ObservableCollection<Contact> contacts;


        public AddressBookRoot()
        {
            contacts = new ObservableCollection<Contact>();
        }


        public IReadOnlyList<Contact> Contacts => contacts;


        public Contact AddNewContact()
        {
            var contact = new Contact();
            AddContact(contact);
            return contact;
        }

        public void AddContact(Contact contact)
        {
            contacts.Add(contact);
        }

        public void RemoveContact(Contact contact)
        {
            contacts.Remove(contact);
        }
    }
}
