using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Domain;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.AddressBook.Modules.Applications.Controllers
{
    /// <summary>
    /// Responsible for the contact management and the master / detail views.
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ContactController
    {
        private readonly IShellService shellService;
        private readonly ContactLayoutViewModel contactLayoutViewModel;
        private readonly DelegateCommand deleteContactCommand;

        [ImportingConstructor]
        public ContactController(IShellService shellService, ContactLayoutViewModel contactLayoutViewModel, ContactListViewModel contactListViewModel, ContactViewModel contactViewModel)
        {
            this.shellService = shellService;
            this.contactLayoutViewModel = contactLayoutViewModel;
            ContactListViewModel = contactListViewModel;
            ContactViewModel = contactViewModel;
            NewContactCommand = new DelegateCommand(NewContact);
            deleteContactCommand = new DelegateCommand(DeleteContact, CanDeleteContact);
        }

        public AddressBookRoot Root { get; set; }

        public ICommand NewContactCommand { get; }

        public ICommand DeleteContactCommand => deleteContactCommand;

        internal ContactListViewModel ContactListViewModel { get; }

        internal ContactViewModel ContactViewModel { get; }

        public void Initialize()
        {
            ContactListViewModel.Contacts = Root.Contacts;
            ContactListViewModel.DeleteContactCommand = DeleteContactCommand;

            PropertyChangedEventManager.AddHandler(ContactListViewModel, ContactListViewModelPropertyChanged, "");

            contactLayoutViewModel.ContactListView = ContactListViewModel.View;
            contactLayoutViewModel.ContactView = ContactViewModel.View;
        }

        public void Run()
        {
            shellService.ContentView = contactLayoutViewModel.View;
        }

        public void Shutdown()
        {
            // Set the views to null so that the garbage collector can collect them.
            contactLayoutViewModel.ContactListView = null;
            contactLayoutViewModel.ContactView = null;
        }

        private void NewContact()
        {
            Contact newContact = Root.AddNewContact();
            ContactListViewModel.SelectedContact = newContact;
            ContactListViewModel.FocusItem();
        }

        private bool CanDeleteContact() { return ContactListViewModel.SelectedContact != null; }

        private void DeleteContact()
        {
            // Use the ContactCollectionView, which represents the sorted/filtered state of the contacts, to determine the next contact to select.
            var nextContact = CollectionHelper.GetNextElementOrDefault(ContactListViewModel.ContactCollectionView, ContactListViewModel.SelectedContact);
            
            Root.RemoveContact(ContactListViewModel.SelectedContact);

            ContactListViewModel.SelectedContact = nextContact ?? ContactListViewModel.ContactCollectionView.LastOrDefault();
            ContactListViewModel.FocusItem();
        }

        private void ContactListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ContactListViewModel.SelectedContact))
            {
                ContactViewModel.Contact = ContactListViewModel.SelectedContact;
                deleteContactCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
