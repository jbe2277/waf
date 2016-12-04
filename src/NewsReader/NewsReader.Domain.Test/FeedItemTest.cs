using Jbe.NewsReader.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.UnitTesting;
using Test.NewsReader.Domain.UnitTesting;

namespace Test.NewsReader.Domain
{
    [TestClass]
    public class FeedItemTest : DomainTest
    {
        [TestMethod]
        public void ApplyValuesFromTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new FeedItem(null, DateTimeOffset.Now, "test", "test", "test"));

            var itemA1 = new FeedItem(new Uri("http://www.test.com/rss/feed"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc", "author");
            Assert.AreEqual(new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), itemA1.Date);
            Assert.AreEqual("name", itemA1.Name);
            Assert.AreEqual("desc", itemA1.Description);
            Assert.AreEqual("author", itemA1.Author);

            var itemA2 = new FeedItem(new Uri("http://www.test.com/rss/feed"), new DateTimeOffset(2022, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name2", "desc2", "author2");
            itemA1.ApplyValuesFrom(itemA2);
            Assert.AreEqual(new DateTimeOffset(2022, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), itemA1.Date);
            Assert.AreEqual("name2", itemA1.Name);
            Assert.AreEqual("desc2", itemA1.Description);
            Assert.AreEqual("author2", itemA1.Author);

            var itemB1 = new FeedItem(new Uri("http://www.test.com/rss/feed2"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc", "author");
            AssertHelper.ExpectedException<InvalidOperationException>(() => itemA1.ApplyValuesFrom(itemB1));
        }

        [TestMethod]
        public void CloneTest()
        {
            var item = new FeedItem(new Uri("http://www.test.com/rss/feed"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc", "author");
            item.MarkAsRead = true;
            var clone = item.Clone();

            Assert.AreNotSame(item, clone);
            Assert.AreEqual(new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), clone.Date);
            Assert.AreEqual("name", clone.Name);
            Assert.AreEqual("desc", clone.Description);
            Assert.AreEqual("author", clone.Author);
            Assert.IsTrue(clone.MarkAsRead);
        }

        [TestMethod]
        public void SupportNull()
        {
            var item = new FeedItem(new Uri("http://www.test.com/rss/feed"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc", "author");
            item.Name = null;
            item.Description = null;
            item.Author = null;
            Assert.IsNull(item.Name);
            Assert.IsNull(item.Description);
            Assert.IsNull(item.Author);
        }
    }
}
