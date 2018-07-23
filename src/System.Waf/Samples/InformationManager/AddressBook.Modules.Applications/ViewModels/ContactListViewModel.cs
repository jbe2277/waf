using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Domain;
using System.Windows.Input;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ContactListViewModel : ViewModel<IContactListView>
    {
        private Contact selectedContact;
        private string filterText = "";

        [ImportingConstructor]
        public ContactListViewModel(IContactListView view) : base(view)
        {
        }

        public IReadOnlyList<Contact> Contacts { get; set; }

        public Contact SelectedContact
        {
            get { return selectedContact; }
            set { SetProperty(ref selectedContact, value); }
        }

        public ICommand DeleteContactCommand { get; set; }

        public string FilterText
        {
            get { return filterText; }
            set { SetProperty(ref filterText, value); }
        }

        public void FocusItem()
        {
            ViewCore.FocusItem();
        }

        public bool Filter(Contact contact)
        {
            if (string.IsNullOrEmpty(filterText)) { return true; }
            
            return (!string.IsNullOrEmpty(contact.Firstname) && contact.Firstname.IndexOf(filterText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(contact.Lastname) && contact.Lastname.IndexOf(filterText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(contact.Email) && contact.Email.IndexOf(filterText, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }
    }
}
