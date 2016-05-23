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


        public FeedManager()
        {
            // Note: Serializer does not call the constructor.
            feeds = new ObservableCollection<Feed>()
            {
                new Feed(new Uri("http://blogs.windows.com/buildingapps/feed/")),
            };
        }


        public ObservableCollection<Feed> Feeds => feeds;
    }
}
