using System.Linq;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Applications.SampleData;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.DesignData
{
    public class SampleContactViewModel : ContactViewModel
    {
        public SampleContactViewModel() : base(new MockContactView())
        {
            Contact = SampleDataProvider.CreateContacts().First();
        }
        

        private class MockContactView : IContactView
        {
            public object DataContext { get; set; }
        }
    }
}
