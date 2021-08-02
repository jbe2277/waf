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
        private Contact? selectedContact;
        private string filterText = "";

        [ImportingConstructor]
        public ContactListViewModel(IContactListView view) : base(view)
        {
        }

        public IReadOnlyList<Contact> Contacts { get; set; } = null!;

        public Contact? SelectedContact
        {
            get => selectedContact;
            set => SetProperty(ref selectedContact, value);
        }

        public ICommand DeleteContactCommand { get; set; } = DelegateCommand.DisabledCommand;

        public string FilterText
        {
            get => filterText;
            set => SetProperty(ref filterText, value);
        }

        public void FocusItem() => ViewCore.FocusItem();

        public bool Filter(Contact contact)
        {
            return string.IsNullOrEmpty(filterText)
                || (!string.IsNullOrEmpty(contact.Firstname) && contact.Firstname.Contains(filterText, StringComparison.CurrentCultureIgnoreCase))
                || (!string.IsNullOrEmpty(contact.Lastname) && contact.Lastname.Contains(filterText, StringComparison.CurrentCultureIgnoreCase))
                || (!string.IsNullOrEmpty(contact.Email) && contact.Email.Contains(filterText, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
