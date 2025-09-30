using System.ServiceModel.Syndication;

namespace UITest.FeedService;

public class SyndicationData
{
    public SyndicationData()
    {
        Feed = new("FeedTitle", "FeedDescription", new Uri("https://sample.com/feed/rss"), "FeedId", DateTime.Now,
        [
            new SyndicationItem("ItemTitle", "ItemContent", new Uri("https://sample.com/posts/1"), "ItemId", DateTime.Now) { PublishDate = DateTime.Now },
        ]);
    }

    public SyndicationFeed Feed { get; }
}