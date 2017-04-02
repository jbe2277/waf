using Jbe.NewsReader.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Composition;
using System.Linq;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Applications.Services
{
    [Export, Shared]
    public class SelectionService : Model
    {
        private readonly Dictionary<Feed, FeedItem> lastSelectedFeedItems;
        private FeedManager feedManager;
        private Feed selectedFeed;
        private FeedItem selectedFeedItem;


        public SelectionService()
        {
            lastSelectedFeedItems = new Dictionary<Feed, FeedItem>();
            SelectedFeeds = new ObservableCollection<Feed>();
            SelectedFeeds.CollectionChanged += SelectedFeedsCollectionChanged;
        }


        internal FeedManager FeedManager
        {
            get { return feedManager; }
            set
            {
                feedManager = value;
                feedManager.Feeds.CollectionChanged += FeedsCollectionChanged;
            }
        }
        
        public ObservableCollection<Feed> SelectedFeeds { get; }

        public Feed SelectedFeed
        {
            get { return selectedFeed; }
            private set
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

        public void SelectFeed(Feed feedToSelect)
        {
            if (SelectedFeeds.Any()) SelectedFeeds.Clear();
            if (feedToSelect != null) SelectedFeeds.Add(feedToSelect);
        }

        private void FeedsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var feed in e.OldItems.Cast<Feed>())
                {
                    lastSelectedFeedItems.Remove(feed);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add) { }
            else if (e.Action == NotifyCollectionChangedAction.Move) { }
            else
            {
                throw new NotSupportedException("FeedsCollectionChanged: " + e.Action);
            }
        }

        private void SelectedFeedsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedFeed = SelectedFeeds.FirstOrDefault();
        }
    }
}