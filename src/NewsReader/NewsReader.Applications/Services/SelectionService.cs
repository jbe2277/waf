using Jbe.NewsReader.Domain;
using System.Collections.Generic;
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
        private Dictionary<Feed, FeedItem> lastSelectedFeedItems;


        public SelectionService()
        {
            lastSelectedFeedItems = new Dictionary<Feed, FeedItem>();
        }


        public Feed SelectedFeed
        {
            get { return selectedFeed; }
            set
            {
                if (SetProperty(ref selectedFeed, value))
                {
                    FeedItem itemToSelect;
                    if (selectedFeed == null || !lastSelectedFeedItems.TryGetValue(selectedFeed, out itemToSelect))
                    {
                        itemToSelect = selectedFeed?.Items.FirstOrDefault();
                    }
                    SelectedFeedItem = itemToSelect;
                }
            }
        }

        public FeedItem SelectedFeedItem
        {
            get { return selectedFeedItem; }
            set
            {
                if (SetProperty(ref selectedFeedItem, value) && SelectedFeed != null && selectedFeedItem != null)
                {
                    lastSelectedFeedItems[SelectedFeed] = selectedFeedItem;
                }
            }
        }


        public void SetDefaultSelectedFeedItem(Feed feed, FeedItem feedItem)
        {
            if (feedItem != null && !lastSelectedFeedItems.ContainsKey(feed))
            {
                lastSelectedFeedItems[feed] = feedItem;
            }
        }
    }
}
