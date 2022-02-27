using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views;

[Export(typeof(IPop3SettingsView)), PartCreationPolicy(CreationPolicy.NonShared)]
public class MockPop3SettingsView : MockView, IPop3SettingsView
{
}
