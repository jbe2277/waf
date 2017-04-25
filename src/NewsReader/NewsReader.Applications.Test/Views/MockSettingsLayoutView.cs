using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.UnitTesting.Mocks;

namespace Test.NewsReader.Applications.Views
{
    [Export(typeof(ISettingsLayoutView)), Export, Shared]
    public class MockSettingsLayoutView : MockView, ISettingsLayoutView
    {
    }
}
