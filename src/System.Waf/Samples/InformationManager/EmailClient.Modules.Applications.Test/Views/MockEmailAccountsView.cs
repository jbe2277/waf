using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views;

[Export(typeof(IEmailAccountsView)), PartCreationPolicy(CreationPolicy.NonShared)]
public class MockEmailAccountsView : MockDialogView<MockEmailAccountsView>, IEmailAccountsView
{
}
