using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Presentation.Views;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.DesignData;

public class SampleContactLayoutViewModel : ContactLayoutViewModel
{
    public SampleContactLayoutViewModel() : base(new MockContactLayoutView())
    {
        ContactListView = new SampleContactListViewModel(new ContactListView()).View;
        ContactView = new SampleContactViewModel(new ContactView()).View;
    }

    private class MockContactLayoutView : IContactLayoutView
    {
        public object? DataContext { get; set; }
    }
}
