using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Domain;
using System;
using System.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Web.Syndication;

namespace Jbe.NewsReader.Applications.Controllers
{
    [Export, Shared]
    internal class NewsFeedsController
    {
        private readonly SelectionService selectionService;
        private readonly SyndicationClient client;


        [ImportingConstructor]
        public NewsFeedsController(SelectionService selectionService)
        {
            this.selectionService = selectionService;
            this.client = new SyndicationClient();
        }


        public FeedManager FeedManager { get; set; }


        public async void Run()
        {
            // Workaround for a x:Bind bug during startup: it restores sometimes the previous value during a TwoWay roundtrip sync. 
            // In this case: selectionService.SelectedFeed = null.
            await Task.Delay(10);

            var tasks = FeedManager.Feeds.ToArray().Select(x => LoadFeedAsync(x));
            await Task.WhenAll(tasks);
        }

        public async Task LoadFeedAsync(Feed feed)
        {
            try
            {
                if (feed.Uri.IsAbsoluteUri && (feed.Uri.Scheme == "http" || feed.Uri.Scheme == "https"))
                {
                    if (selectionService.SelectedFeed == null)
                    {
                        selectionService.SelectedFeed = feed;
                    }
                    var syndicationFeed = await client.RetrieveFeedAsync(feed.Uri);
                    var items = syndicationFeed.Items.Select(x => new FeedItem(
                            x.ItemUri ?? x.Links.FirstOrDefault()?.Uri,
                            x.PublishedDate,
                            x.Title.Text,
                            RemoveHtmlTags(x.Summary?.Text),
                            x.Authors.FirstOrDefault()?.NodeValue
                        )).ToArray();

                    feed.Name = syndicationFeed.Title.Text;
                    feed.UpdateItems(items);
                    if (selectionService.SelectedFeed == feed)
                    {
                        selectionService.SelectedFeedItem = feed.Items.FirstOrDefault();
                    }
                }
                else
                {
                    feed.LoadErrorMessage = ResourceLoader.GetForViewIndependentUse().GetString("UrlMustBeginWithHttp");
                    feed.LoadError = new InvalidOperationException(@"The URL must begin with http:// or https://");
                }
            }
            catch (Exception ex)
            {
                feed.LoadErrorMessage = ResourceLoader.GetForViewIndependentUse().GetString("ErrorLoadRssFeed");
                feed.LoadError = ex;
            }
        }

        private static string RemoveHtmlTags(string message)
        {
            if (string.IsNullOrEmpty(message)) { return message; }
            return Regex.Replace(Regex.Replace(message, "\\&.{0,4}\\;", ""), "<.*?>", "");
        }
    }
}
