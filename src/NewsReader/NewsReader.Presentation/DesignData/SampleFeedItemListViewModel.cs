using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;

namespace Jbe.NewsReader.Presentation.DesignData
{
    public class SampleFeedItemListViewModel : FeedItemListViewModel
    {
        public SampleFeedItemListViewModel() : base(new MockFeedItemListView(), new SelectionService())
        {
            // Note: Design time data does not work with {x:Bind}
        }


        private class MockFeedItemListView : IFeedItemListView
        {
            public object DataContext { get; set; }
        }
    }
}
