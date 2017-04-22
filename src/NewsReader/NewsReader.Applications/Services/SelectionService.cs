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
        private bool? selectAllFeeds;
        private FeedItem selectedFeedItem;
        private bool? selectAllFeedItems;


        public SelectionService()
        {
            lastSelectedFeedItems = new Dictionary<Feed, FeedItem>();
            SelectedFeeds = new ObservableCollection<Feed>();
            SelectedFeedItems = new ObservableCollection<FeedItem>();
            SelectedFeeds.CollectionChanged += SelectedFeedsCollectionChanged;
            SelectedFeedItems.CollectionChanged += SelectedFeedItemsCollectionChanged;
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
                var oldValue = selectedFeed;
                if (SetProperty(ref selectedFeed, value))
                {
                    if (oldValue != null) oldValue.Items.CollectionChanged -= FeedItemsCollectionChanged;

                    FeedItem itemToSelect;
                    if (selectedFeed == null || !lastSelectedFeedItems.TryGetValue(selectedFeed, out itemToSelect))
                    {
                        itemToSelect = selectedFeed?.Items.FirstOrDefault();
                    }
                    SelectFeedItem(itemToSelect);

                    if (value != null) value.Items.CollectionChanged += FeedItemsCollectionChanged;
                    UpdateSelectAllFeedItems();
                }
            }
        }

        public bool? SelectAllFeeds
        {
            get { return selectAllFeeds; }
            set
            {
                if (SetProperty(ref selectAllFeeds, value))
                {
                    if (value == true)
                    {
                        FeedManager.Feeds.Except(SelectedFeeds).ToList().ForEach(x => SelectedFeeds.Add(x));
                    }
                    else if (value == false)
                    {
                        SelectFeed(null);
                    }
                }
            }
        }
        
        public ObservableCollection<FeedItem> SelectedFeedItems { get; }

        public FeedItem SelectedFeedItem
        {
            get { return selectedFeedItem; }
            private set
            {
                if (SetProperty(ref selectedFeedItem, value) && SelectedFeed != null && selectedFeedItem != null)
                {
                    lastSelectedFeedItems[SelectedFeed] = selectedFeedItem;
                }
            }
        }

        public bool? SelectAllFeedItems
        {
            get { return selectAllFeedItems; }
            set
            {
                if (SetProperty(ref selectAllFeedItems, value))
                {
                    if (value == true)
                    {
                        SelectedFeed?.Items.Except(SelectedFeedItems).ToList().ForEach(x => SelectedFeedItems.Add(x));
                    }
                    else if (value == false)
                    {
                        SelectFeedItem(null);
                    }
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

        public void SelectFeedItem(FeedItem feedItemToSelect)
        {
            if (SelectedFeedItems.Any()) SelectedFeedItems.Clear();
            if (feedItemToSelect != null) SelectedFeedItems.Add(feedItemToSelect);
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
            UpdateSelectAllFeeds();
        }

        private void FeedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateSelectAllFeedItems();
        }

        private void UpdateSelectAllFeeds()
        {
            var newValue = !SelectedFeeds.Any() ? false : (SelectedFeeds.Count == FeedManager.Feeds.Count ? true : (bool?)null);
            if (selectAllFeeds != newValue)
            {
                selectAllFeeds = newValue;
                RaisePropertyChanged(nameof(SelectAllFeeds));
            }
        }

        private void UpdateSelectAllFeedItems()
        {
            var newValue = !SelectedFeedItems.Any() ? false : (SelectedFeedItems.Count == SelectedFeed?.Items.Count ? true : (bool?)null);
            if (selectAllFeedItems != newValue)
            {
                selectAllFeedItems = newValue;
                RaisePropertyChanged(nameof(SelectAllFeedItems));
            }
        }

        private void SelectedFeedsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedFeed = SelectedFeeds.FirstOrDefault();
            UpdateSelectAllFeeds();
        }

        private void SelectedFeedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedFeedItem = SelectedFeedItems.FirstOrDefault();
            UpdateSelectAllFeedItems();
        }
    }
}