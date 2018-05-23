using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.AddressBook.Modules.Domain
{
    [DataContract]
    public class AddressBookRoot : ValidatableModel
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
            contact.Validate();
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

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var contact in Contacts) contact.Validate();
        }
    }
}
