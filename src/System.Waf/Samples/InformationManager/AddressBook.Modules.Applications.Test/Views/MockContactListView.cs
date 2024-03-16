using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Test.InformationManager.AddressBook.Modules.Applications.Views;

[Export(typeof(IContactListView)), PartCreationPolicy(CreationPolicy.NonShared)]
public class MockContactListView : MockView<ContactListViewModel>, IContactListView
{
    public Action<MockContactListView>? FocusItemAction { get; set; }

    public void FocusItem() => FocusItemAction?.Invoke(this);
}
