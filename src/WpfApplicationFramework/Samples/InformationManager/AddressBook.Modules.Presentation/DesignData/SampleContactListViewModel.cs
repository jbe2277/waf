using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Domain;
using Waf.InformationManager.AddressBook.Modules.Applications.SampleData;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.DesignData
{
    public class SampleContactListViewModel : ContactListViewModel
    {
        public SampleContactListViewModel() : base(new MockContactListView())
        {
            Contacts = SampleDataProvider.CreateContacts();
            SelectedContact = Contacts.First();
            FilterText = "My filter text";
        }
        

        private class MockContactListView : IContactListView
        {
            public object DataContext { get; set; }
            
            public void FocusItem() { }
        }
    }
}
