using Waf.InformationManager.AddressBook.Interfaces.Applications;
using Waf.InformationManager.AddressBook.Interfaces.Domain;

namespace Test.InformationManager.AddressBook.Modules.Applications.Services;

public class MockAddressBookService : IAddressBookService
{
    public Func<object, ContactDto?>? ShowSelectContactViewAction { get; set; }

    public ContactDto? ShowSelectContactView(object ownerView) => ShowSelectContactViewAction?.Invoke(ownerView);
}
