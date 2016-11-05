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
        [TestMethod]
        public void RemoveFeed()
        {
            var feedManager = new FeedManager();
            var selectionService = Container.GetExport<SelectionService>();

            var controller = Container.GetExport<NewsFeedsController>();
            controller.FeedManager = feedManager;

            // Wait for Run to udate the first Feed
            controller.Run();
            Context.WaitFor(() => feedManager.Feeds.Single().Items.Count == 3, TimeSpan.FromSeconds(1));


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
    }
}
