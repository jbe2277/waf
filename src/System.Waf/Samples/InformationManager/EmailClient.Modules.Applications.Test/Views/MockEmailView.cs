using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views;

[Export(typeof(IEmailView)), PartCreationPolicy(CreationPolicy.NonShared)]
public class MockEmailView : MockView, IEmailView
{
}
