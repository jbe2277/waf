using Waf.NewsReader.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.NewsReader.Domain.UnitTesting;
using static Waf.NewsReader.Domain.FeedManager;

namespace Test.NewsReader.Domain;

[TestClass]
public class FeedManagerTest : DomainTest
{
    [TestMethod]
    public void SetDataManager()
    {
        var feedManagerA = new FeedManager();
        Assert.AreSame(feedManagerA, feedManagerA.Feeds.Single().DataManager);

        var feedA1 = new Feed(new Uri("http://www.test.com/rss/feed"));
        Assert.IsNull(feedA1.DataManager);

        feedManagerA.Feeds.Add(feedA1);
        Assert.AreSame(feedManagerA, feedA1.DataManager);

        feedManagerA.Feeds.Remove(feedA1);
        Assert.IsNull(feedA1.DataManager);

        // Now use the serialzer with one Feed added

        feedManagerA.Feeds.Add(feedA1);
        var feedManagerB = SerializerHelper.Clone(feedManagerA);
        Assert.AreSame(feedManagerB, feedManagerB.Feeds.First().DataManager);

        var feedB1 = feedManagerB.Feeds.Last();
        Assert.AreSame(feedManagerB, feedB1.DataManager);

        feedManagerB.Feeds.Remove(feedB1);
        Assert.IsNull(feedB1.DataManager);

        feedManagerB.Feeds.Add(feedB1);
        Assert.AreSame(feedManagerB, feedB1.DataManager);
    }

    [TestMethod]
    public void MergeTest()
    {
        var feedManagerA = new FeedManager();
        feedManagerA.Feeds.Remove(feedManagerA.Feeds.Single());
        var feedA1 = new Feed(new Uri("http://www.test.com/rss/feedA1"));
        var feedA2 = new Feed(new Uri("http://www.test.com/rss/feedA2"));
        feedManagerA.Feeds.Add(feedA1);
        feedManagerA.Feeds.Add(feedA2);
        var item = new FeedItem(new Uri("http://www.test.com/rss/feed"), new DateTimeOffset(2120, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc");
        feedA2.UpdateItems(new[] { item });
        Assert.IsFalse(feedManagerA.Feeds.Last().Items.Single().MarkAsRead);

        var feedManagerB = new FeedManager()
        {
            ItemLifetime = TimeSpan.FromDays(42),
            MaxItemsLimit = 43
        };
        feedManagerB.Feeds.Remove(feedManagerB.Feeds.Single());
        var feedB1 = new Feed(new Uri("http://www.test.com/rss/feedB1"));
        var feedB2 = new Feed(new Uri("http://www.test.com/rss/feedA2"));
        feedManagerB.Feeds.Add(feedB1);
        feedManagerB.Feeds.Add(feedB2);
        feedB2.UpdateItems(new[] { item }, cloneItemsBeforeInsert: true);
        feedB2.Items.Single().MarkAsRead = true;

        feedManagerA.Merge(feedManagerB);

        Assert.AreEqual(42, feedManagerA.ItemLifetime!.Value.Days);
        Assert.AreEqual(43u, feedManagerA.MaxItemsLimit!.Value);
        Assert.IsTrue(new[] { "http://www.test.com/rss/feedB1", "http://www.test.com/rss/feedA2" }.SequenceEqual(feedManagerA.Feeds.Select(x => x.Uri.ToString())));
        Assert.IsTrue(feedManagerA.Feeds.Last().Items.Single().MarkAsRead);


        // Just remove one element => this way the Merge does not create new instances
        var feedManagerC = new FeedManager();
        feedManagerC.Feeds.Remove(feedManagerC.Feeds.Single());
        var feedC2 = new Feed(new Uri("http://www.test.com/rss/feedA2"));
        feedManagerC.Feeds.Add(feedC2);
        feedC2.UpdateItems(new[] { item }, cloneItemsBeforeInsert: true);
        feedC2.Items.Single().MarkAsRead = false;
        var itemA = feedManagerA.Feeds.Last().Items.Single();

        feedManagerA.Merge(feedManagerC);

        Assert.IsTrue(new[] { "http://www.test.com/rss/feedA2" }.SequenceEqual(feedManagerA.Feeds.Select(x => x.Uri.ToString())));
        Assert.AreSame(itemA, feedManagerA.Feeds.Last().Items.Single());
        Assert.IsFalse(itemA.MarkAsRead);
    }

    [TestMethod]
    public void FeedEqualityComparerTest()
    {
        Assert.IsTrue(FeedEqualityComparer.Default.Equals(new Feed(new Uri("http://microsoft.com")), new Feed(new Uri("http://microsoft.com"))));
        Assert.IsFalse(FeedEqualityComparer.Default.Equals(new Feed(new Uri("http://microsoft.com")), new Feed(new Uri("http://google.com"))));
        Assert.IsFalse(FeedEqualityComparer.Default.Equals(new Feed(new Uri("http://microsoft.com")), new Feed(null!)));
        Assert.IsFalse(FeedEqualityComparer.Default.Equals(new Feed(null!), new Feed(new Uri("http://microsoft.com"))));
        Assert.IsFalse(FeedEqualityComparer.Default.Equals(new Feed(new Uri("http://microsoft.com")), null!));
        Assert.IsFalse(FeedEqualityComparer.Default.Equals(null!, new Feed(new Uri("http://microsoft.com"))));
        Assert.IsTrue(FeedEqualityComparer.Default.Equals(new Feed(null!), new Feed(null!)));
        Assert.IsTrue(FeedEqualityComparer.Default.Equals(null!, null!));

        Assert.AreEqual(0, FeedEqualityComparer.Default.GetHashCode(new Feed(null!)));
        Assert.AreEqual(0, FeedEqualityComparer.Default.GetHashCode(null!));
    }
}