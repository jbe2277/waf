using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.UnitTesting.Mocks;

namespace Test.NewsReader.Applications.Views
{
    [Export(typeof(IFeedItemView)), Export, Shared]
    public class MockFeedItemView : MockView, IFeedItemView
    {
    }
}
