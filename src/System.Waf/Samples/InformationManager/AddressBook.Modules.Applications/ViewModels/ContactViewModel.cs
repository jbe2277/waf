using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

public class ContactViewModel : ViewModel<IContactView>
{
    private Contact? contact;

    public ContactViewModel(IContactView view) : base(view)
    {
    }

    public Contact? Contact
    {
        get => contact;
        set => SetProperty(ref contact, value);
    }
}
