using Waf.NewsReader.Domain;
using Xunit;
using Comparer = Waf.NewsReader.Domain.FeedManager.FeedEqualityComparer;

namespace Test.NewsReader.Domain;

public class FeedManagerTest
{
    [Fact]
    public void SetDataManager()
    {
        var feedManagerA = new FeedManager();
        Assert.Same(feedManagerA, feedManagerA.Feeds.Single().DataManager);

        var feedA1 = new Feed(new("http://www.test.com/rss/feed"));
        Assert.Null(feedA1.DataManager);

        feedManagerA.Feeds.Add(feedA1);
        Assert.Same(feedManagerA, feedA1.DataManager);

        feedManagerA.Feeds.Remove(feedA1);
        Assert.Null(feedA1.DataManager);

        // Now use the serialzer with one Feed added

        feedManagerA.Feeds.Add(feedA1);
        var feedManagerB = SerializerHelper.Clone(feedManagerA);
        Assert.Same(feedManagerB, feedManagerB.Feeds.First().DataManager);

        var feedB1 = feedManagerB.Feeds.Last();
        Assert.Same(feedManagerB, feedB1.DataManager);

        feedManagerB.Feeds.Remove(feedB1);
        Assert.Null(feedB1.DataManager);

        feedManagerB.Feeds.Add(feedB1);
        Assert.Same(feedManagerB, feedB1.DataManager);
    }

    [Fact]
    public void MergeTest()
    {
        var feedManagerA = new FeedManager();
        feedManagerA.Feeds.Remove(feedManagerA.Feeds.Single());
        var feedA1 = new Feed(new("http://www.test.com/rss/feedA1"));
        var feedA2 = new Feed(new("http://www.test.com/rss/feedA2"));
        feedManagerA.Feeds.Add(feedA1);
        feedManagerA.Feeds.Add(feedA2);
        var item = new FeedItem(new("http://www.test.com/rss/feed"), new(2120, 5, 5, 12, 0, 0, new(1, 0, 0)), "name", "desc");
        feedA2.UpdateItems([ item ]);
        Assert.False(feedManagerA.Feeds.Last().Items.Single().MarkAsRead);

        var feedManagerB = new FeedManager()
        {
            ItemLifetime = TimeSpan.FromDays(42),
            MaxItemsLimit = 43
        };
        feedManagerB.Feeds.Remove(feedManagerB.Feeds.Single());
        var feedB1 = new Feed(new("http://www.test.com/rss/feedB1"));
        var feedB2 = new Feed(new("http://www.test.com/rss/feedA2"));
        feedManagerB.Feeds.Add(feedB1);
        feedManagerB.Feeds.Add(feedB2);
        feedB2.UpdateItems([ item ], cloneItemsBeforeInsert: true);
        feedB2.Items.Single().MarkAsRead = true;

        feedManagerA.Merge(feedManagerB);

        Assert.Equal(42, feedManagerA.ItemLifetime!.Value.Days);
        Assert.Equal(43u, feedManagerA.MaxItemsLimit!.Value);
        Assert.Equal([ "http://www.test.com/rss/feedB1", "http://www.test.com/rss/feedA2" ], feedManagerA.Feeds.Select(x => x.Uri.ToString()));
        Assert.True(feedManagerA.Feeds.Last().Items.Single().MarkAsRead);


        // Just remove one element => this way the Merge does not create new instances
        var feedManagerC = new FeedManager();
        feedManagerC.Feeds.Remove(feedManagerC.Feeds.Single());
        var feedC2 = new Feed(new("http://www.test.com/rss/feedA2"));
        feedManagerC.Feeds.Add(feedC2);
        feedC2.UpdateItems([ item ], cloneItemsBeforeInsert: true);
        feedC2.Items.Single().MarkAsRead = false;
        var itemA = feedManagerA.Feeds.Last().Items.Single();

        feedManagerA.Merge(feedManagerC);

        Assert.Equal([ "http://www.test.com/rss/feedA2" ], feedManagerA.Feeds.Select(x => x.Uri.ToString()));
        Assert.Same(itemA, feedManagerA.Feeds.Last().Items.Single());
        Assert.False(itemA.MarkAsRead);
    }

    [Fact]
    public void FeedEqualityComparerTest()
    {
        Assert.True(Comparer.Default.Equals(new(new("http://microsoft.com")), new(new("http://microsoft.com"))));
        Assert.False(Comparer.Default.Equals(new(new("http://microsoft.com")), new(new("http://google.com"))));
        Assert.False(Comparer.Default.Equals(new(new("http://microsoft.com")), new(null!)));
        Assert.False(Comparer.Default.Equals(new(null!), new(new("http://microsoft.com"))));
        Assert.False(Comparer.Default.Equals(new(new("http://microsoft.com")), null!));
        Assert.False(Comparer.Default.Equals(null!, new(new("http://microsoft.com"))));
        Assert.True(Comparer.Default.Equals(new(null!), new(null!)));
        Assert.True(Comparer.Default.Equals(null!, null!));

        Assert.Equal(0, Comparer.Default.GetHashCode(new(null!)));
        Assert.Equal(0, Comparer.Default.GetHashCode(null!));
    }
}