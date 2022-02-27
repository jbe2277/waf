using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views;

[Export(typeof(IExchangeSettingsView)), PartCreationPolicy(CreationPolicy.NonShared)]
public class MockExchangeSettingsView : MockView, IExchangeSettingsView
{
}
