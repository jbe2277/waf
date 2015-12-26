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
        private readonly ContactListViewModel contactListViewModel;
        private readonly ContactViewModel contactViewModel;
        private readonly DelegateCommand newContactCommand;
        private readonly DelegateCommand deleteContactCommand;

        
        [ImportingConstructor]
        public ContactController(IShellService shellService, ContactLayoutViewModel contactLayoutViewModel, ContactListViewModel contactListViewModel, ContactViewModel contactViewModel)
        {
            this.shellService = shellService;
            this.contactLayoutViewModel = contactLayoutViewModel;
            this.contactListViewModel = contactListViewModel;
            this.contactViewModel = contactViewModel;
            this.newContactCommand = new DelegateCommand(NewContact);
            this.deleteContactCommand = new DelegateCommand(DeleteContact, CanDeleteContact);
        }


        public AddressBookRoot Root { get; set; }

        public ICommand NewContactCommand { get { return newContactCommand; } }

        public ICommand DeleteContactCommand { get { return deleteContactCommand; } }

        internal ContactListViewModel ContactListViewModel { get { return contactListViewModel; } }

        internal ContactViewModel ContactViewModel { get { return contactViewModel; } }


        public void Initialize()
        {
            contactListViewModel.Contacts = Root.Contacts;
            contactListViewModel.DeleteContactCommand = DeleteContactCommand;

            PropertyChangedEventManager.AddHandler(contactListViewModel, ContactListViewModelPropertyChanged, "");

            contactLayoutViewModel.ContactListView = contactListViewModel.View;
            contactLayoutViewModel.ContactView = contactViewModel.View;
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
            contactListViewModel.SelectedContact = newContact;
            contactListViewModel.FocusItem();
        }

        private bool CanDeleteContact() { return contactListViewModel.SelectedContact != null; }

        private void DeleteContact()
        {
            // Use the ContactCollectionView, which represents the sorted/filtered state of the contacts, to determine the next contact to select.
            var nextContact = CollectionHelper.GetNextElementOrDefault(contactListViewModel.ContactCollectionView, contactListViewModel.SelectedContact);
            
            Root.RemoveContact(contactListViewModel.SelectedContact);

            contactListViewModel.SelectedContact = nextContact ?? contactListViewModel.ContactCollectionView.LastOrDefault();
            contactListViewModel.FocusItem();
        }

        private void ContactListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedContact")
            {
                contactViewModel.Contact = contactListViewModel.SelectedContact != null ? contactListViewModel.SelectedContact : null;
                deleteContactCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
