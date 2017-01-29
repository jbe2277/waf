using Jbe.NewsReader.Applications.Controllers;
using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Waf.UnitTesting;
using Test.NewsReader.Applications.Services;
using Test.NewsReader.Applications.UnitTesting;

namespace Test.NewsReader.Applications.Controllers
{
    [TestClass]
    public class NewsFeedsControllerTest : ApplicationsTest
    {
        private FeedManager feedManager;
        private SelectionService selectionService;
        private NewsFeedsController controller;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            feedManager = new FeedManager();
            selectionService = Container.GetExport<SelectionService>();

            controller = Container.GetExport<NewsFeedsController>();
            controller.FeedManager = feedManager;

            // Wait for Run to udate the first Feed
            controller.Run();
            Context.WaitFor(() => feedManager.Feeds.Single().Items.Count == 3, TimeSpan.FromSeconds(1));
        }


        [TestMethod]
        public void RunAndUpdateAndRefresh()
        {
            var item = feedManager.Feeds.Single().Items[0];
            Assert.AreEqual(item, selectionService.SelectedFeedItem);
            controller.ReadUnreadCommand.Execute("read");
            Assert.IsTrue(item.MarkAsRead);

            // Update

            var syndicationService = Container.GetExport<MockSyndicationService>();
            var firstFeed = MockSyndicationClient.CreateSampleFeed();
            syndicationService.LastCreatedMockClient.RetrieveFeedAsyncStub = uri => Task.FromResult(new FeedDto("Sample Feed", new[]
            {
                new FeedItemDto(new Uri("http://www.test.com/rss/feed9"), new DateTimeOffset(2021, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name 9", "desc 9"),
            }.Concat(firstFeed.Items).ToArray()));

            controller.Update();
            Context.WaitFor(() => feedManager.Feeds.Single().Items.Count == 4, TimeSpan.FromSeconds(1));
            Assert.IsTrue(feedManager.Feeds.Single().Items.Single(x => x.Uri == item.Uri).MarkAsRead);

            // Refresh

            syndicationService.LastCreatedMockClient.RetrieveFeedAsyncStub = uri => Task.FromResult(new FeedDto("Sample Feed", new[]
            {
                new FeedItemDto(new Uri("http://www.test.com/rss/feed9"), new DateTimeOffset(2021, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name 10", "desc 9"),
            }.Concat(firstFeed.Items).ToArray()));

            controller.RefreshFeedCommand.Execute(null);
            Context.Wait(TimeSpan.FromMilliseconds(500));
            Context.WaitFor(() => feedManager.Feeds.Single().Items[0].Name == "name 10", TimeSpan.FromSeconds(1));

            AssertHelper.CanExecuteChangedEvent(controller.RefreshFeedCommand, () => selectionService.SelectedFeed = null);
            Assert.IsFalse(controller.RefreshFeedCommand.CanExecute(null));
        }

        [TestMethod]
        public void RemoveFeed()
        {
            // Remove but cancel confirmation
            Assert.AreEqual(feedManager.Feeds.Single(), selectionService.SelectedFeed);
            Assert.IsTrue(controller.RemoveFeedCommand.CanExecute(null));

            var messageService = Container.GetExport<MockMessageService>();
            var confirmationShown = false;
            var confirmationResult = false;
            messageService.ShowYesNoQuestionDialogAsyncStub = msg =>
            {
                confirmationShown = true;
                return Task.FromResult(confirmationResult);
            };
            
            controller.RemoveFeedCommand.Execute(null);
            Assert.IsTrue(confirmationShown);
            Assert.AreEqual(1, feedManager.Feeds.Count);


            // Remove
            Assert.AreEqual(feedManager.Feeds.Single(), selectionService.SelectedFeed);
            Assert.IsTrue(controller.RemoveFeedCommand.CanExecute(null));

            confirmationShown = false;
            confirmationResult = true;
            controller.RemoveFeedCommand.Execute(null);

            Context.WaitFor(() => feedManager.Feeds.Count == 0, TimeSpan.FromSeconds(1));
            Assert.IsFalse(controller.RemoveFeedCommand.CanExecute(null));
            Assert.IsTrue(confirmationShown);
        }

        [TestMethod]
        public void ReadUnreadCommand()
        {
            var item = feedManager.Feeds.Single().Items[0];
            Assert.AreEqual(item, selectionService.SelectedFeedItem);
            Assert.IsFalse(item.MarkAsRead);

            controller.ReadUnreadCommand.Execute(null);
            Assert.IsTrue(item.MarkAsRead);
            
            controller.ReadUnreadCommand.Execute("unread");
            Assert.IsFalse(item.MarkAsRead);

            controller.ReadUnreadCommand.Execute("unread");
            Assert.IsFalse(item.MarkAsRead);

            controller.ReadUnreadCommand.Execute("read");
            Assert.IsTrue(item.MarkAsRead);

            controller.ReadUnreadCommand.Execute("read");
            Assert.IsTrue(item.MarkAsRead);

            AssertHelper.CanExecuteChangedEvent(controller.ReadUnreadCommand, () => selectionService.SelectedFeedItem = null);
            Assert.IsFalse(controller.ReadUnreadCommand.CanExecute(null));
        }
    }
}
