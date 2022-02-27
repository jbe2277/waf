using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Foundation;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Applications.Controllers;

[Export, PartCreationPolicy(CreationPolicy.NonShared)]
internal class SelectContactController
{
    private readonly SelectContactViewModel selectContactViewModel;
    private readonly DelegateCommand selectContactCommand;
    private ObservableListView<Contact> contactsView = null!;
    private IWeakEventProxy contactListViewModelPropertyChangedProxy = null!;

    [ImportingConstructor]
    public SelectContactController(SelectContactViewModel selectContactViewModel, ContactListViewModel contactListViewModel)
    {
        this.selectContactViewModel = selectContactViewModel;
        ContactListViewModel = contactListViewModel;
        selectContactCommand = new DelegateCommand(SelectContact, CanSelectContact);
    }

    public object OwnerView { get; set; } = null!;

    public AddressBookRoot Root { get; set; } = null!;

    public Contact? SelectedContact { get; private set; }

    internal ContactListViewModel ContactListViewModel { get; }

    public void Initialize()
    {
        contactsView = new ObservableListView<Contact>(Root.Contacts, null, ContactListViewModel.Filter, null);
        ContactListViewModel.Contacts = contactsView;
        ContactListViewModel.SelectedContact = Root.Contacts.FirstOrDefault();
        selectContactViewModel.ContactListView = ContactListViewModel.View;
        selectContactViewModel.OkCommand = selectContactCommand;
        contactListViewModelPropertyChangedProxy = WeakEvent.PropertyChanged.Add(ContactListViewModel, ContactListViewModelPropertyChanged);
    }

    public void Run() => selectContactViewModel.ShowDialog(OwnerView);

    public void Shutdown()
    {
        contactListViewModelPropertyChangedProxy.Remove();
        contactsView.Dispose();
    }

    private bool CanSelectContact() => ContactListViewModel.SelectedContact != null;

    private void SelectContact()
    {
        SelectedContact = ContactListViewModel.SelectedContact;
        selectContactViewModel.Close();
    }

    private void ContactListViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ContactListViewModel.SelectedContact)) selectContactCommand.RaiseCanExecuteChanged();
        else if (e.PropertyName == nameof(ContactListViewModel.FilterText)) contactsView.Update();
    }
}
