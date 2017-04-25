using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.UnitTesting.Mocks;
using System;

namespace Test.NewsReader.Applications.Views
{
    [Export(typeof(IFeedItemListView)), Export, Shared]
    public class MockFeedItemListView : MockView, IFeedItemListView
    {
        public void CancelMultipleSelectionMode()
        {
        }
    }
}
