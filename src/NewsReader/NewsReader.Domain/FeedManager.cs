using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Domain
{
    [DataContract]
    public class FeedManager : Model, IDataManager
    {
        [DataMember] private readonly ObservableCollection<Feed> feeds;
        [DataMember] private TimeSpan? itemLifetime;
        [DataMember] private uint? maxItemsLimit;


        public FeedManager()
        {
            // Note: Serializer does not call the constructor.
            feeds = new ObservableCollection<Feed>()
            {
                new Feed(new Uri("http://blogs.windows.com/buildingapps/feed/")),
            };
            ItemLifetime = TimeSpan.FromDays(365);
            MaxItemsLimit = 250;
            Initialize();
        }


        public ObservableCollection<Feed> Feeds => feeds;

        public TimeSpan? ItemLifetime
        {
            get { return itemLifetime; }
            set { SetProperty(ref itemLifetime, value); }
        }

        public uint? MaxItemsLimit
        {
            get { return maxItemsLimit; }
            set { SetProperty(ref maxItemsLimit, value); }
        }


        public void Merge(FeedManager secondFeedManager)
        {
            // TODO: secondFeedManager wins; improve Merge so that local offline changes are not lost.
            ItemLifetime = secondFeedManager.ItemLifetime;
            MaxItemsLimit = secondFeedManager.MaxItemsLimit;

            foreach (var feed in Feeds.ToArray())
            {
                Feeds.Remove(feed);
            }
            foreach (var feed in secondFeedManager.Feeds)
            {
                Feeds.Add(feed);
            }
        }

        private void Initialize()
        {
            feeds.CollectionChanged += FeedsCollectionChanged;
            foreach (var feed in feeds)
            {
                feed.DataManager = this;
            }
        }

        private void FeedsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                throw new NotSupportedException("The Reset action is not supported.");
            }
            foreach (var newFeed in e.NewItems?.Cast<Feed>() ?? CollectionHelper.Empty<Feed>())
            {
                newFeed.DataManager = this;
            }
            foreach (var oldFeed in e.OldItems?.Cast<Feed>() ?? CollectionHelper.Empty<Feed>())
            {
                oldFeed.DataManager = null;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Initialize();
        }
    }
}
