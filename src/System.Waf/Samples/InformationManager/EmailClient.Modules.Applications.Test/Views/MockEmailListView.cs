using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views;

public class MockEmailListView : MockView<EmailListViewModel>, IEmailListView
{
    public Action<MockEmailListView>? FocusItemAction { get; set; }

    public void FocusItem() => FocusItemAction?.Invoke(this);
}
