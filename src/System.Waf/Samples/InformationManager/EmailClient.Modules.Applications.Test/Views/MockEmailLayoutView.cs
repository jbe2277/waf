using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views
{
    [Export(typeof(IEmailLayoutView)), Export]
    public class MockEmailLayoutView : MockView, IEmailLayoutView
    {
    }
}
