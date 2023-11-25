using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Applications.SampleData;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.DesignData;

public class SampleContactListViewModel : ContactListViewModel
{
    public SampleContactListViewModel() : this(new MockContactListView())
    {
    }

    public SampleContactListViewModel(IContactListView view) : base(view)
    {
        Contacts = SampleDataProvider.CreateContacts();
        SelectedContact = Contacts[0];
        FilterText = "My filter text";
    }


    private sealed class MockContactListView : IContactListView
    {
        public object? DataContext { get; set; }

        public void FocusItem() { }
    }
}
