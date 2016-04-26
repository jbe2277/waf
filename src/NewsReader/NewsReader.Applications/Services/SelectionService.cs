using Jbe.NewsReader.Domain;
using System.Composition;
using System.Linq;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Applications.Services
{
    [Export, Shared]
    public class SelectionService : Model
    {
        private Feed selectedFeed;
        private FeedItem selectedFeedItem;


        public Feed SelectedFeed
        {
            get { return selectedFeed; }
            set
            {
                if (SetProperty(ref selectedFeed, value))
                {
                    SelectedFeedItem = selectedFeed?.Items?.FirstOrDefault();
                }
            }
        }

        public FeedItem SelectedFeedItem
        {
            get { return selectedFeedItem; }
            set { SetProperty(ref selectedFeedItem, value); }
        }
    }
}
