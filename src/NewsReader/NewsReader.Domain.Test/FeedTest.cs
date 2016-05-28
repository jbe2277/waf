using Jbe.NewsReader.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Test.NewsReader.Domain
{
    [TestClass]
    public class FeedTest
    {
        [TestMethod]
        public void NameTest()
        {
            var feed = new Feed(new Uri("http://www.test.com/rss/feed"));
            Assert.AreEqual("http://www.test.com/rss/feed", feed.Name);
            feed.Name = "name";
            Assert.AreEqual("name", feed.Name);
        }

        [TestMethod]
        public void IsLoadingTest1() => IsLoadingCoreTest(false);

        [TestMethod]
        public void IsLoadingTest2() => IsLoadingCoreTest(true);

        private void IsLoadingCoreTest(bool useSerializer)
        {
            var feed1 = new Feed(new Uri("http://www.test.com/rss/feed"));
            var feed2 = new Feed(new Uri("http://www.test.com/rss/feed"));
            feed1 = !useSerializer ? feed1 : SerializerHelper.Clone(feed1);
            feed2 = !useSerializer ? feed2 : SerializerHelper.Clone(feed2);

            Assert.IsTrue(feed1.IsLoading);
            feed1.LoadError = new InvalidOperationException("test");
            Assert.IsFalse(feed1.IsLoading);

            Assert.IsTrue(feed2.IsLoading);
            feed2.UpdateItems(new FeedItem[0]);
            Assert.IsFalse(feed2.IsLoading);
        }

        [TestMethod]
        public void UpdateItemsTest1() => UpdateItemsCoreTest(false);

        [TestMethod]
        public void UpdateItemsTest2() => UpdateItemsCoreTest(true);

        private void UpdateItemsCoreTest(bool useSerializer)
        {
            var feed = new Feed(new Uri("http://www.test.com/rss/feed"));
            feed.UpdateItems(new[] {
                new FeedItem(new Uri("http://www.test.com/rss/feed/1"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name1", "desc", "author"),
                new FeedItem(new Uri("http://www.test.com/rss/feed/2"), new DateTimeOffset(2020, 5, 7, 12, 0, 0, new TimeSpan(1, 0, 0)), "name2", "desc", "author"),
            });
            feed = !useSerializer ? feed : SerializerHelper.Clone(feed);

            Assert.AreEqual(2, feed.Items.Count);
            Assert.IsTrue(new[] { "name2", "name1" }.SequenceEqual(feed.Items.Select(x => x.Name)));

            feed.UpdateItems(new[] {
                new FeedItem(new Uri("http://www.test.com/rss/feed/2"), new DateTimeOffset(2020, 5, 7, 12, 0, 0, new TimeSpan(1, 0, 0)), "name2b", "desc", "author"),
                new FeedItem(new Uri("http://www.test.com/rss/feed/3"), new DateTimeOffset(2020, 5, 6, 12, 0, 0, new TimeSpan(1, 0, 0)), "name3", "desc", "author"),
            });

            Assert.AreEqual(3, feed.Items.Count);
            Assert.IsTrue(new[] { "name2b", "name3", "name1" }.SequenceEqual(feed.Items.Select(x => x.Name)));
        }

        [TestMethod]
        public void UnreadItemsCountTest1() => UpdateItemsCoreTest(false);

        [TestMethod]
        public void UnreadItemsCountTest2() => UpdateItemsCoreTest(false);

        private void UnreadItemsCountCoreTest(bool useSerializer)
        {
            var feed = new Feed(new Uri("http://www.test.com/rss/feed"));
            feed.UpdateItems(new[] {
                new FeedItem(new Uri("http://www.test.com/rss/feed/1"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name1", "desc", "author"),
                new FeedItem(new Uri("http://www.test.com/rss/feed/2"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name2", "desc", "author"),
            });
            feed = !useSerializer ? feed : SerializerHelper.Clone(feed);
            
            Assert.AreEqual(2, feed.UnreadItemsCount);

            feed.Items[0].MarkAsRead = true;

            Assert.AreEqual(1, feed.UnreadItemsCount);
        }
    }
}
