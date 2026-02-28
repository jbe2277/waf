using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

public class ContactLayoutViewModel(IContactLayoutView view) : ViewModel<IContactLayoutView>(view)
{
    public object? ContactListView { get; set => SetProperty(ref field, value); }

    public object? ContactView { get; set => SetProperty(ref field, value); }
}
