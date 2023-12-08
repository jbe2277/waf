using Waf.NewsReader.Domain;
using System.Waf.UnitTesting;
using Xunit;

namespace Test.NewsReader.Domain;

public class FeedItemTest
{
    [Fact]
    public void ApplyValuesFromTest()
    {
        AssertHelper.ExpectedException<ArgumentNullException>(() => new FeedItem(null!, DateTimeOffset.Now, "test", "test"));

        var itemA1 = new FeedItem(new("http://www.test.com/rss/feed"), new(2020, 5, 5, 12, 0, 0, new(1, 0, 0)), "name", "desc");
        Assert.Equal(new(2020, 5, 5, 12, 0, 0, new(1, 0, 0)), itemA1.Date);
        Assert.Equal("name", itemA1.Name);
        Assert.Equal("desc", itemA1.Description);
        Assert.False(itemA1.MarkAsRead);

        var itemA2 = new FeedItem(new("http://www.test.com/rss/feed"), new(2022, 5, 5, 12, 0, 0, new(1, 0, 0)), "name2", "desc2");
        itemA2.MarkAsRead = true;
        itemA1.ApplyValuesFrom(itemA2);
        Assert.Equal(new(2022, 5, 5, 12, 0, 0, new(1, 0, 0)), itemA1.Date);
        Assert.Equal("name2", itemA1.Name);
        Assert.Equal("desc2", itemA1.Description);
        Assert.True(itemA1.MarkAsRead);

        var itemB1 = new FeedItem(new("http://www.test.com/rss/feed2"), new(2020, 5, 5, 12, 0, 0, new(1, 0, 0)), "name", "desc");
        AssertHelper.ExpectedException<InvalidOperationException>(() => itemA1.ApplyValuesFrom(itemB1));
    }

    [Fact]
    public void CloneTest()
    {
        var item = new FeedItem(new("http://www.test.com/rss/feed"), new(2020, 5, 5, 12, 0, 0, new(1, 0, 0)), "name", "desc");
        item.MarkAsRead = true;
        var clone = item.Clone();

        Assert.NotSame(item, clone);
        Assert.Equal(new(2020, 5, 5, 12, 0, 0, new(1, 0, 0)), clone.Date);
        Assert.Equal("name", clone.Name);
        Assert.Equal("desc", clone.Description);
        Assert.True(clone.MarkAsRead);
    }

    [Fact]
    public void SupportNull()
    {
        var item = new FeedItem(new("http://www.test.com/rss/feed"), new(2020, 5, 5, 12, 0, 0, new(1, 0, 0)), "name", "desc");
        item.Name = null;
        item.Description = null;
        Assert.Null(item.Name);
        Assert.Null(item.Description);
    }
}
