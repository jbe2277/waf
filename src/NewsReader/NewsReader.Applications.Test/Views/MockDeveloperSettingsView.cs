using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.UnitTesting.Mocks;

namespace Test.NewsReader.Applications.Views
{
    [Export(typeof(IDeveloperSettingsView)), Export, Shared]
    public class MockDeveloperSettingsView : MockView, IDeveloperSettingsView
    {
    }
}
