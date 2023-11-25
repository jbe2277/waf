using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Applications.SampleData;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.DesignData;

public class SampleContactViewModel : ContactViewModel
{
    public SampleContactViewModel() : this(new MockContactView())
    {
    }

    public SampleContactViewModel(IContactView view) : base(view)
    {
        Contact = SampleDataProvider.CreateContacts()[0];
    }


    private sealed class MockContactView : IContactView
    {
        public object? DataContext { get; set; }
    }
}
