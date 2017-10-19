using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Presentation.Views;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.DesignData
{
    public class SampleSelectContactViewModel : SelectContactViewModel
    {
        public SampleSelectContactViewModel() : base(new MockSelectContactView())
        {
            ContactListView = new SampleContactListViewModel(new ContactListView()).View;
        }


        private class MockSelectContactView : ISelectContactView
        {
            public object DataContext { get; set; }

            public void Close() { }
            
            public void ShowDialog(object owner) { }
        }
    }
}
