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
        private readonly ContactListViewModel contactListViewModel;
        private readonly DelegateCommand selectContactCommand;
        

        [ImportingConstructor]
        public SelectContactController(SelectContactViewModel selectContactViewModel, ContactListViewModel contactListViewModel)
        {
            this.selectContactViewModel = selectContactViewModel;
            this.contactListViewModel = contactListViewModel;
            this.selectContactCommand = new DelegateCommand(SelectContact, CanSelectContact);
        }


        public object OwnerView { get; set; }

        public AddressBookRoot Root { get; set; }

        public Contact SelectedContact { get; private set; }

        internal ContactListViewModel ContactListViewModel { get { return contactListViewModel; } }


        public void Initialize()
        {
            contactListViewModel.Contacts = Root.Contacts;
            contactListViewModel.SelectedContact = Root.Contacts.FirstOrDefault();
            selectContactViewModel.ContactListView = contactListViewModel.View;
            selectContactViewModel.OkCommand = selectContactCommand;

            PropertyChangedEventManager.AddHandler(contactListViewModel, ContactListViewModelPropertyChanged, "");
        }

        public void Run()
        {
            selectContactViewModel.ShowDialog(OwnerView);
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(contactListViewModel, ContactListViewModelPropertyChanged, "");
        }

        private bool CanSelectContact() { return contactListViewModel.SelectedContact != null; }

        private void SelectContact()
        {
            SelectedContact = contactListViewModel.SelectedContact;
            selectContactViewModel.Close();
        }

        private void ContactListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedContact")
            {
                selectContactCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
