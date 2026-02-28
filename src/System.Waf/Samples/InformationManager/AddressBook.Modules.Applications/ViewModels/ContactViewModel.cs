using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

public class ContactViewModel(IContactView view) : ViewModel<IContactView>(view)
{
    public Contact? Contact { get; set => SetProperty(ref field, value); }
}
