using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SelectContactController
    {
        private readonly SelectContactViewModel selectContactViewModel;
        private readonly DelegateCommand selectContactCommand;
        
        [ImportingConstructor]
        public SelectContactController(SelectContactViewModel selectContactViewModel, ContactListViewModel contactListViewModel)
        {
            this.selectContactViewModel = selectContactViewModel;
            ContactListViewModel = contactListViewModel;
            selectContactCommand = new DelegateCommand(SelectContact, CanSelectContact);
        }

        public object OwnerView { get; set; }

        public AddressBookRoot Root { get; set; }

        public Contact SelectedContact { get; private set; }

        internal ContactListViewModel ContactListViewModel { get; }

        public void Initialize()
        {
            ContactListViewModel.Contacts = Root.Contacts;
            ContactListViewModel.SelectedContact = Root.Contacts.FirstOrDefault();
            selectContactViewModel.ContactListView = ContactListViewModel.View;
            selectContactViewModel.OkCommand = selectContactCommand;

            PropertyChangedEventManager.AddHandler(ContactListViewModel, ContactListViewModelPropertyChanged, "");
        }

        public void Run()
        {
            selectContactViewModel.ShowDialog(OwnerView);
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(ContactListViewModel, ContactListViewModelPropertyChanged, "");
        }

        private bool CanSelectContact() { return ContactListViewModel.SelectedContact != null; }

        private void SelectContact()
        {
            SelectedContact = ContactListViewModel.SelectedContact;
            selectContactViewModel.Close();
        }

        private void ContactListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ContactListViewModel.SelectedContact))
            {
                selectContactCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
