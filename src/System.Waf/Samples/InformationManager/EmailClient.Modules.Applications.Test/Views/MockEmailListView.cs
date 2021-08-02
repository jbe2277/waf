using System;
using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views
{
    [Export(typeof(IEmailListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MockEmailListView : MockView, IEmailListView
    {
        public Action<MockEmailListView>? FocusItemAction { get; set; }

        public void FocusItem() => FocusItemAction?.Invoke(this);
    }
}
