using System;
using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Test.InformationManager.AddressBook.Modules.Applications.Views
{
    [Export(typeof(IContactListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MockContactListView : MockView, IContactListView
    {
        public Action<MockContactListView>? FocusItemAction { get; set; }
        
        public void FocusItem()
        {
            FocusItemAction?.Invoke(this);
        }
    }
}
