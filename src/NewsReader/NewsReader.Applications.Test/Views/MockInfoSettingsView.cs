using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.UnitTesting.Mocks;

namespace Test.NewsReader.Applications.Views
{
    [Export(typeof(IInfoSettingsView)), Export, Shared]
    public class MockInfoSettingsView : MockView, IInfoSettingsView
    {
    }
}
