using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views;

[Export(typeof(IBasicEmailAccountView)), PartCreationPolicy(CreationPolicy.NonShared)]
public class MockBasicEmailAccountView : MockView<BasicEmailAccountViewModel>, IBasicEmailAccountView
{
}
