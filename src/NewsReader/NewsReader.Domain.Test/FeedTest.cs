using Waf.NewsReader.Domain;
using System.Waf.UnitTesting;
using Xunit;

namespace Test.NewsReader.Domain;

public class FeedTest
{
    [Fact]
    public void IsLoadingTest1() => IsLoadingCoreTest(false);

    [Fact]
    public void IsLoadingTest2() => IsLoadingCoreTest(true);

    private static void IsLoadingCoreTest(bool useSerializer)
    {
        var feed1 = new Feed(new("http://www.test.com/rss/feed"));
        var feed2 = new Feed(new("http://www.test.com/rss/feed"));
        feed1 = !useSerializer ? feed1 : SerializerHelper.Clone(feed1);
        feed2 = !useSerializer ? feed2 : SerializerHelper.Clone(feed2);

        var feedManager = new FeedManager();
        feedManager.Feeds.Add(feed1);
        feedManager.Feeds.Add(feed2);

        Assert.False(feed1.IsLoading);
        feed1.StartLoading();
        Assert.True(feed1.IsLoading);
        feed1.SetLoadError(new InvalidOperationException("test"), "display test");
        Assert.False(feed1.IsLoading);
        Assert.Equal("test", feed1.LoadError!.Message);
        Assert.Equal("display test", feed1.LoadErrorMessage);
        feed1.StartLoading();
        Assert.Null(feed1.LoadError);
        Assert.Null(feed1.LoadErrorMessage);

        Assert.False(feed2.IsLoading);
        feed2.StartLoading();
        Assert.True(feed2.IsLoading);
        feed2.UpdateItems([]);
        Assert.False(feed2.IsLoading);
    }

    [Fact]
    public void UpdateItemsTest1() => UpdateItemsCoreTest(false, false);

    [Fact]
    public void UpdateItemsTest2() => UpdateItemsCoreTest(false, true);

    [Fact]
    public void UpdateItemsTest3() => UpdateItemsCoreTest(true, false);

    [Fact]
    public void UpdateItemsTest4() => UpdateItemsCoreTest(true, true);

    private static void UpdateItemsCoreTest(bool cloneItemsBeforeInsert, bool useSerializer)
    {
        var feed = new Feed(new("http://www.test.com/rss/feed"));
        feed.UpdateItems([
            new FeedItem(new("http://www.test.com/rss/feed/1"), new(2020, 5, 5, 12, 0, 0, new(1, 0, 0)), "name1", "desc"),
            new FeedItem(new("http://www.test.com/rss/feed/2"), new(2020, 5, 7, 12, 0, 0, new(1, 0, 0)), "name2", "desc"),
        ]);
        feed = !useSerializer ? feed : SerializerHelper.Clone(feed);

        Assert.Equal(2, feed.Items.Count);
        Assert.Equal([ "name2", "name1" ], feed.Items.Select(x => x.Name));

        var newItems = new[]
        {
            new FeedItem(new("http://www.test.com/rss/feed/2"), new(2020, 5, 7, 12, 0, 0, new(1, 0, 0)), "name2b", "desc"),
            new FeedItem(new("http://www.test.com/rss/feed/3"), new(2020, 5, 6, 12, 0, 0, new(1, 0, 0)), "name3", "desc"),
        };
        feed.UpdateItems(newItems, cloneItemsBeforeInsert: cloneItemsBeforeInsert);

        Assert.Equal(3, feed.Items.Count);
        Assert.Equal([ "name2b", "name3", "name1" ], feed.Items.Select(x => x.Name));
        if (cloneItemsBeforeInsert)
        {
            Assert.NotSame(feed.Items[1], newItems[1]);
        }
        else
        {
            Assert.Same(feed.Items[1], newItems[1]);
        }
    }

    [Fact]
    public void UnreadItemsCountTest1() => UnreadItemsCountCoreTest(false);

    [Fact]
    public void UnreadItemsCountTest2() => UnreadItemsCountCoreTest(true);

    private static void UnreadItemsCountCoreTest(bool useSerializer)
    {
        var feed = new Feed(new("http://www.test.com/rss/feed"));
        var feedManager = new FeedManager() { MaxItemsLimit = null, ItemLifetime = null };
        feedManager.Feeds.Add(feed);
        feed.UpdateItems([
            new FeedItem(new("http://www.test.com/rss/feed/1"), new(2020, 5, 5, 12, 0, 0, new(1, 0, 0)), "name1", "desc"),
            new FeedItem(new("http://www.test.com/rss/feed/2"), new(2020, 5, 5, 12, 0, 0, new(1, 0, 0)), "name2", "desc"),
        ]);
        feed = !useSerializer ? feed : SerializerHelper.Clone(feed);

        Assert.Equal(2, feed.UnreadItemsCount);

        feed.Items[0].MarkAsRead = true;

        Assert.Equal(1, feed.UnreadItemsCount);

        AssertHelper.PropertyChangedEvent(feed, x => x.UnreadItemsCount, () =>
        {
            feed.UpdateItems([new FeedItem(new("http://www.test.com/rss/feed/3"), new(2020, 5, 5, 12, 0, 3, new(1, 0, 0)), "name3", "desc")]);
        });

        Assert.Equal(2, feed.UnreadItemsCount);

        AssertHelper.PropertyChangedEvent(feed, x => x.UnreadItemsCount, () => feed.Items[2].MarkAsRead = true);

        Assert.Equal(1, feed.UnreadItemsCount);
    }

    [Fact]
    public void TrimItemsListWithMaxItemsLimitTest1() => TrimItemsListWithMaxItemsLimitTest(false);

    [Fact]
    public void TrimItemsListWithMaxItemsLimitTest2() => TrimItemsListWithMaxItemsLimitTest(true);

    private static void TrimItemsListWithMaxItemsLimitTest(bool useSerializer)
    {
        var feed = new Feed(new("http://www.test.com/rss/feed"));
        feed = !useSerializer ? feed : SerializerHelper.Clone(feed);

        var feedManager = new FeedManager();
        feedManager.Feeds.Add(feed);

        UpdateFeedItems(feed);
        Assert.Equal([ "name3", "name2", "name1" ], feed.Items.Select(x => x.Name));

        feedManager.MaxItemsLimit = 2;
        Assert.Equal([ "name3", "name2" ], feed.Items.Select(x => x.Name));

        UpdateFeedItems(feed);
        Assert.Equal([ "name3", "name2" ], feed.Items.Select(x => x.Name));

        feedManager.MaxItemsLimit = 3;
        UpdateFeedItems(feed);
        Assert.Equal([ "name3", "name2", "name1" ], feed.Items.Select(x => x.Name));

        feedManager.MaxItemsLimit = 1;
        Assert.Equal([ "name3" ], feed.Items.Select(x => x.Name));

        feedManager.MaxItemsLimit = 0;
        Assert.Empty(feed.Items.Select(x => x.Name));
    }

    [Fact]
    public void TrimItemsListWithItemLifetimeTest1() => TrimItemsListWithItemLifetimeTest(false);

    [Fact]
    public void TrimItemsListWithItemLifetimeTest2() => TrimItemsListWithItemLifetimeTest(false);

    private static void TrimItemsListWithItemLifetimeTest(bool useSerializer)
    {
        var feed = new Feed(new("http://www.test.com/rss/feed"));
        feed = !useSerializer ? feed : SerializerHelper.Clone(feed);

        var feedManager = new FeedManager();
        feedManager.Feeds.Add(feed);

        UpdateFeedItems(feed);
        Assert.Equal([ "name3", "name2", "name1" ], feed.Items.Select(x => x.Name));

        feedManager.ItemLifetime = TimeSpan.FromDays(6);
        Assert.Equal([ "name3", "name2" ], feed.Items.Select(x => x.Name));

        UpdateFeedItems(feed);
        Assert.Equal([ "name3", "name2" ], feed.Items.Select(x => x.Name));

        feedManager.ItemLifetime = TimeSpan.FromDays(11);
        UpdateFeedItems(feed);
        Assert.Equal([ "name3", "name2", "name1" ], feed.Items.Select(x => x.Name));

        feedManager.ItemLifetime = TimeSpan.FromDays(2);
        Assert.Equal([ "name3" ], feed.Items.Select(x => x.Name));

        feedManager.ItemLifetime = TimeSpan.FromDays(0.5);
        Assert.Empty(feed.Items.Select(x => x.Name));
    }

    private static void UpdateFeedItems(Feed feed)
    {
        feed.UpdateItems([
            new FeedItem(new("http://www.test.com/rss/feed/1"), DateTimeOffset.Now - TimeSpan.FromDays(10), "name1", "desc"),
            new FeedItem(new("http://www.test.com/rss/feed/2"), DateTimeOffset.Now - TimeSpan.FromDays(5), "name2", "desc"),
            new FeedItem(new("http://www.test.com/rss/feed/3"), DateTimeOffset.Now - TimeSpan.FromDays(1), "name3", "desc"),
        ]);
    }
}
