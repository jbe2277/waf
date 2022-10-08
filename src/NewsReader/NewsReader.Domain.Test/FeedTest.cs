using Waf.NewsReader.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Test.NewsReader.Domain.UnitTesting;

namespace Test.NewsReader.Domain
{
    [TestClass]
    public class FeedTest : DomainTest
    {
        [TestMethod]
        public void IsLoadingTest1() => IsLoadingCoreTest(false);

        [TestMethod]
        public void IsLoadingTest2() => IsLoadingCoreTest(true);

        private static void IsLoadingCoreTest(bool useSerializer)
        {
            var feed1 = new Feed(new Uri("http://www.test.com/rss/feed"));
            var feed2 = new Feed(new Uri("http://www.test.com/rss/feed"));
            feed1 = !useSerializer ? feed1 : SerializerHelper.Clone(feed1);
            feed2 = !useSerializer ? feed2 : SerializerHelper.Clone(feed2);

            var feedManager = new FeedManager();
            feedManager.Feeds.Add(feed1);
            feedManager.Feeds.Add(feed2);

            Assert.IsFalse(feed1.IsLoading);
            feed1.StartLoading();
            Assert.IsTrue(feed1.IsLoading);
            feed1.SetLoadError(new InvalidOperationException("test"), "display test");
            Assert.IsFalse(feed1.IsLoading);
            Assert.AreEqual("test", feed1.LoadError!.Message);
            Assert.AreEqual("display test", feed1.LoadErrorMessage);
            feed1.StartLoading();
            Assert.IsNull(feed1.LoadError);
            Assert.IsNull(feed1.LoadErrorMessage);

            Assert.IsFalse(feed2.IsLoading);
            feed2.StartLoading();
            Assert.IsTrue(feed2.IsLoading);
            feed2.UpdateItems(new FeedItem[0]);
            Assert.IsFalse(feed2.IsLoading);
        }

        [TestMethod]
        public void UpdateItemsTest1() => UpdateItemsCoreTest(false, false);

        [TestMethod]
        public void UpdateItemsTest2() => UpdateItemsCoreTest(false, true);

        [TestMethod]
        public void UpdateItemsTest3() => UpdateItemsCoreTest(true, false);

        [TestMethod]
        public void UpdateItemsTest4() => UpdateItemsCoreTest(true, true);

        private static void UpdateItemsCoreTest(bool cloneItemsBeforeInsert, bool useSerializer)
        {
            var feed = new Feed(new Uri("http://www.test.com/rss/feed"));
            feed.UpdateItems(new[] {
                new FeedItem(new Uri("http://www.test.com/rss/feed/1"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name1", "desc"),
                new FeedItem(new Uri("http://www.test.com/rss/feed/2"), new DateTimeOffset(2020, 5, 7, 12, 0, 0, new TimeSpan(1, 0, 0)), "name2", "desc"),
            });
            feed = !useSerializer ? feed : SerializerHelper.Clone(feed);

            Assert.AreEqual(2, feed.Items.Count);
            Assert.IsTrue(new[] { "name2", "name1" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            var newItems = new[]
            {
                new FeedItem(new Uri("http://www.test.com/rss/feed/2"), new DateTimeOffset(2020, 5, 7, 12, 0, 0, new TimeSpan(1, 0, 0)), "name2b", "desc"),
                new FeedItem(new Uri("http://www.test.com/rss/feed/3"), new DateTimeOffset(2020, 5, 6, 12, 0, 0, new TimeSpan(1, 0, 0)), "name3", "desc"),
            };
            feed.UpdateItems(newItems, cloneItemsBeforeInsert: cloneItemsBeforeInsert);

            Assert.AreEqual(3, feed.Items.Count);
            Assert.IsTrue(new[] { "name2b", "name3", "name1" }.SequenceEqual(feed.Items.Select(x => x.Name)));
            if (cloneItemsBeforeInsert)
            {
                Assert.AreNotSame(feed.Items[1], newItems[1]);
            }
            else
            {
                Assert.AreSame(feed.Items[1], newItems[1]);
            }
        }

        [TestMethod]
        public void UnreadItemsCountTest1() => UnreadItemsCountCoreTest(false);

        [TestMethod]
        public void UnreadItemsCountTest2() => UnreadItemsCountCoreTest(true);

        private static void UnreadItemsCountCoreTest(bool useSerializer)
        {
            var feed = new Feed(new Uri("http://www.test.com/rss/feed"));
            var feedManager = new FeedManager() { MaxItemsLimit = null, ItemLifetime = null };
            feedManager.Feeds.Add(feed);
            feed.UpdateItems(new[] {
                new FeedItem(new Uri("http://www.test.com/rss/feed/1"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name1", "desc"),
                new FeedItem(new Uri("http://www.test.com/rss/feed/2"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name2", "desc"),
            });
            feed = !useSerializer ? feed : SerializerHelper.Clone(feed);

            Assert.AreEqual(2, feed.UnreadItemsCount);

            feed.Items[0].MarkAsRead = true;

            Assert.AreEqual(1, feed.UnreadItemsCount);
        }

        [TestMethod]
        public void TrimItemsListWithMaxItemsLimitTest1() => TrimItemsListWithMaxItemsLimitTest(false);

        [TestMethod]
        public void TrimItemsListWithMaxItemsLimitTest2() => TrimItemsListWithMaxItemsLimitTest(true);

        private static void TrimItemsListWithMaxItemsLimitTest(bool useSerializer)
        {
            var feed = new Feed(new Uri("http://www.test.com/rss/feed"));
            feed = !useSerializer ? feed : SerializerHelper.Clone(feed);

            var feedManager = new FeedManager();
            feedManager.Feeds.Add(feed);

            UpdateFeedItems(feed);
            Assert.IsTrue(new[] { "name3", "name2", "name1" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            feedManager.MaxItemsLimit = 2;
            Assert.IsTrue(new[] { "name3", "name2" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            UpdateFeedItems(feed);
            Assert.IsTrue(new[] { "name3", "name2" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            feedManager.MaxItemsLimit = 3;
            UpdateFeedItems(feed);
            Assert.IsTrue(new[] { "name3", "name2", "name1" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            feedManager.MaxItemsLimit = 1;
            Assert.IsTrue(new[] { "name3" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            feedManager.MaxItemsLimit = 0;
            Assert.IsTrue(new string[0].SequenceEqual(feed.Items.Select(x => x.Name)));
        }

        [TestMethod]
        public void TrimItemsListWithItemLifetimeTest1() => TrimItemsListWithItemLifetimeTest(false);

        [TestMethod]
        public void TrimItemsListWithItemLifetimeTest2() => TrimItemsListWithItemLifetimeTest(false);

        private static void TrimItemsListWithItemLifetimeTest(bool useSerializer)
        {
            var feed = new Feed(new Uri("http://www.test.com/rss/feed"));
            feed = !useSerializer ? feed : SerializerHelper.Clone(feed);

            var feedManager = new FeedManager();
            feedManager.Feeds.Add(feed);

            UpdateFeedItems(feed);
            Assert.IsTrue(new[] { "name3", "name2", "name1" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            feedManager.ItemLifetime = TimeSpan.FromDays(6);
            Assert.IsTrue(new[] { "name3", "name2" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            UpdateFeedItems(feed);
            Assert.IsTrue(new[] { "name3", "name2" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            feedManager.ItemLifetime = TimeSpan.FromDays(11);
            UpdateFeedItems(feed);
            Assert.IsTrue(new[] { "name3", "name2", "name1" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            feedManager.ItemLifetime = TimeSpan.FromDays(2);
            Assert.IsTrue(new[] { "name3" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            feedManager.ItemLifetime = TimeSpan.FromDays(0.5);
            Assert.IsTrue(new string[0].SequenceEqual(feed.Items.Select(x => x.Name)));
        }

        private static void UpdateFeedItems(Feed feed)
        {
            feed.UpdateItems(new[] {
                new FeedItem(new Uri("http://www.test.com/rss/feed/1"), DateTimeOffset.Now - TimeSpan.FromDays(10), "name1", "desc"),
                new FeedItem(new Uri("http://www.test.com/rss/feed/2"), DateTimeOffset.Now - TimeSpan.FromDays(5), "name2", "desc"),
                new FeedItem(new Uri("http://www.test.com/rss/feed/3"), DateTimeOffset.Now - TimeSpan.FromDays(1), "name3", "desc"),
            });
        }
    }
}
