using Jbe.NewsReader.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Test.NewsReader.Domain
{
    [TestClass]
    public class FeedManagerTest
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
    }
}
