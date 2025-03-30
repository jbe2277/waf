using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views;

public class MockEmailAccountsView : MockDialogView<MockEmailAccountsView, EmailAccountsViewModel>, IEmailAccountsView
{
}
