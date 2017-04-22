using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using System.Waf.UnitTesting.Mocks;

namespace Test.NewsReader.Applications.Views
{
    [Export(typeof(IFeedListView)), Export, Shared]
    public class MockFeedListView : MockView, IFeedListView
    {
        public Action FeedAddedStub { get; set; }


        public void FeedAdded()
        {
            FeedAddedStub?.Invoke();
        }

        public void CancelMultipleSelectionMode() { }
    }
}
