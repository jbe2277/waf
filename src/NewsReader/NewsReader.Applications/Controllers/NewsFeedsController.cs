using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Domain;
using System;
using System.Collections.Specialized;
using System.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            FeedManager.Feeds.CollectionChanged += FeedsCollectionChanged;
            foreach (var feed in FeedManager.Feeds.ToArray())
            {
                await LoadFeedAsync(feed);
            }

            // Workaround for a x:Bind bug: it restores sometimes the previous value during a TwoWay roundtrip sync. In this case: null.
            if (selectionService.SelectedFeed == null)
            {
                selectionService.SelectedFeed = FeedManager.Feeds.FirstOrDefault();
            }
        }

        private async void FeedsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Feed feed in e.NewItems)
                {
                    await LoadFeedAsync(feed);
                }
            }
        }

        private async Task LoadFeedAsync(Feed feed)
        {
            try
            {
                if ((feed.Uri.Scheme == "http" || feed.Uri.Scheme == "https"))
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
            }
            catch (Exception ex)
            {
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
