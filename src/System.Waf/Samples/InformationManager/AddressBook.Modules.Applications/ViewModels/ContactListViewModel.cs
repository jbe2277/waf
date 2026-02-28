using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Domain;
using System.Windows.Input;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

public class ContactListViewModel(IContactListView view) : ViewModel<IContactListView>(view)
{
    public IReadOnlyList<Contact> Contacts { get; set; } = null!;

    public Contact? SelectedContact { get; set => SetProperty(ref field, value); }

    public ICommand DeleteContactCommand { get; set; } = DelegateCommand.DisabledCommand;

    public string FilterText { get; set => SetProperty(ref field, value); } = "";

    public void FocusItem() => ViewCore.FocusItem();

    public bool Filter(Contact contact)
    {
        return string.IsNullOrEmpty(FilterText)
            || (!string.IsNullOrEmpty(contact.Firstname) && contact.Firstname.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase))
            || (!string.IsNullOrEmpty(contact.Lastname) && contact.Lastname.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase))
            || (!string.IsNullOrEmpty(contact.Email) && contact.Email.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase));
    }
}
