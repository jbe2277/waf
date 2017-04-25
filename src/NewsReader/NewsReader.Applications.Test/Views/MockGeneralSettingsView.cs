using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.UnitTesting.Mocks;

namespace Test.NewsReader.Applications.Views
{
    [Export(typeof(IGeneralSettingsView)), Export, Shared]
    public class MockGeneralSettingsView : MockView, IGeneralSettingsView
    {
    }
}
