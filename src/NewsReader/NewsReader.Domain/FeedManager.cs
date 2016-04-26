using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Domain
{
    [DataContract]
    public class FeedManager : Model
    {
        [DataMember] private readonly ObservableCollection<Feed> feeds;
        private ReadOnlyObservableList<Feed> readOnlyFeeds;

        public FeedManager()
        {
            // Note: Serializer does not call the constructor.
            feeds = new ObservableCollection<Feed>()
            {
                new Feed(new Uri("http://blogs.windows.com/buildingapps/feed/")),
            };
        }


        public IReadOnlyObservableList<Feed> Feeds => readOnlyFeeds ?? (readOnlyFeeds = new ReadOnlyObservableList<Feed>(feeds));


        public void AddFeed(Feed feed)
        {
            feeds.Add(feed);
        }

        public void RemoveFeed(Feed feed)
        {
            feeds.Remove(feed);
        }
    }
}
