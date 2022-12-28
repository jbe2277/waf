using Waf.NewsReader.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Test.NewsReader.Domain.UnitTesting;

namespace Test.NewsReader.Domain;

[TestClass]
public class FeedItemTest : DomainTest
{
    [TestMethod]
    public void ApplyValuesFromTest()
    {
        AssertHelper.ExpectedException<ArgumentNullException>(() => new FeedItem(null!, DateTimeOffset.Now, "test", "test"));

        var itemA1 = new FeedItem(new Uri("http://www.test.com/rss/feed"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc");
        Assert.AreEqual(new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), itemA1.Date);
        Assert.AreEqual("name", itemA1.Name);
        Assert.AreEqual("desc", itemA1.Description);
        Assert.IsFalse(itemA1.MarkAsRead);

        var itemA2 = new FeedItem(new Uri("http://www.test.com/rss/feed"), new DateTimeOffset(2022, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name2", "desc2");
        itemA2.MarkAsRead = true;
        itemA1.ApplyValuesFrom(itemA2);
        Assert.AreEqual(new DateTimeOffset(2022, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), itemA1.Date);
        Assert.AreEqual("name2", itemA1.Name);
        Assert.AreEqual("desc2", itemA1.Description);
        Assert.IsTrue(itemA1.MarkAsRead);

        var itemB1 = new FeedItem(new Uri("http://www.test.com/rss/feed2"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc");
        AssertHelper.ExpectedException<InvalidOperationException>(() => itemA1.ApplyValuesFrom(itemB1));
    }

    [TestMethod]
    public void CloneTest()
    {
        var item = new FeedItem(new Uri("http://www.test.com/rss/feed"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc");
        item.MarkAsRead = true;
        var clone = item.Clone();

        Assert.AreNotSame(item, clone);
        Assert.AreEqual(new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), clone.Date);
        Assert.AreEqual("name", clone.Name);
        Assert.AreEqual("desc", clone.Description);
        Assert.IsTrue(clone.MarkAsRead);
    }

    [TestMethod]
    public void SupportNull()
    {
        var item = new FeedItem(new Uri("http://www.test.com/rss/feed"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc");
        item.Name = null;
        item.Description = null;
        Assert.IsNull(item.Name);
        Assert.IsNull(item.Description);
    }
}
