using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.Controllers
{
    [Export, Shared]
    internal class NewsFeedsController
    {
        private readonly IResourceService resourceService;
        private readonly IAppService appService;
        private readonly ILauncherService launcherService;
        private readonly IMessageService messageService;
        private readonly INetworkInfoService networkInfoService;
        private readonly SelectionService selectionService;
        private readonly Lazy<FeedListViewModel> feedListViewModel;
        private readonly ISyndicationClient client;
        private readonly AsyncDelegateCommand addNewFeedCommand;
        private readonly AsyncDelegateCommand removeFeedCommand;
        private readonly AsyncDelegateCommand refreshFeedCommand;
        private readonly DelegateCommand readUnreadCommand;
        private readonly AsyncDelegateCommand launchWebBrowserCommand;


        [ImportingConstructor]
        public NewsFeedsController(IResourceService resourceService, IAppService appService, ILauncherService launcherService, IMessageService messageService,
            ISyndicationService syndicationService, INetworkInfoService networkInfoService, SelectionService selectionService, Lazy<FeedListViewModel> feedListViewModel)
        {
            this.resourceService = resourceService;
            this.appService = appService;
            this.launcherService = launcherService;
            this.messageService = messageService;
            this.networkInfoService = networkInfoService;
            this.selectionService = selectionService;
            this.feedListViewModel = feedListViewModel;
            this.client = syndicationService.CreateClient();
            this.addNewFeedCommand = new AsyncDelegateCommand(AddNewFeed);
            this.removeFeedCommand = new AsyncDelegateCommand(RemoveFeedAsync, CanRemoveFeed);
            this.refreshFeedCommand = new AsyncDelegateCommand(RefreshFeed);
            this.readUnreadCommand = new DelegateCommand(MarkAsReadUnread, CanMarkAsReadUnread);
            this.launchWebBrowserCommand = new AsyncDelegateCommand(LaunchWebBrowser, CanLaunchWebBrowser);

            this.selectionService.PropertyChanged += SelectionServicePropertyChanged;
            ((INotifyCollectionChanged)this.selectionService.SelectedFeeds).CollectionChanged += SelectedFeedsCollectionChanged;
        }


        public FeedManager FeedManager { get; set; }

        public ICommand AddNewFeedCommand => addNewFeedCommand;

        public ICommand RemoveFeedCommand => removeFeedCommand;

        public ICommand RefreshFeedCommand => refreshFeedCommand;

        // TODO: Support to mark multiple feeds
        public ICommand ReadUnreadCommand => readUnreadCommand;

        public ICommand LaunchWebBrowserCommand => launchWebBrowserCommand;


        public async void Run()
        {
            // Workaround for a x:Bind bug during startup: it restores sometimes the previous value during a TwoWay roundtrip sync. 
            // In this case: selectionService.SelectedFeed = null.
            await appService.DelayIdleAsync();

            var tasks = FeedManager.Feeds.ToArray().Select(x => LoadFeedAsync(x)).ToArray();
            if (tasks.Length > 0)
            {
                await tasks.First();
            }

            // Ensure that a feed is selected
            if (!selectionService.SelectedFeeds.Any())
            {
                selectionService.SelectFeed(selectionService.FeedManager.Feeds.FirstOrDefault());
            }

            // TODO: Enforce scroll into view after loading more items
            //var itemToSelectAgain = selectionService.SelectedFeedItem;
            //selectionService.SelectedFeedItem = null;
            //selectionService.SelectedFeedItem = itemToSelectAgain;

            await Task.WhenAll(tasks);
            FeedManager.Feeds.CollectionChanged += FeedsCollectionChanged;
        }

        public void Update()
        {
            foreach (var feed in FeedManager.Feeds)
            {
                IgnoreResult(LoadFeedAsync(feed));
            }
        }

        private void FeedsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Feed item in e.NewItems?.Cast<Feed>() ?? CollectionHelper.Empty<Feed>())
            {
                IgnoreResult(LoadFeedAsync(item));
            }
        }

        private static void IgnoreResult(Task task) { }

        private async Task LoadFeedAsync(Feed feed, bool ignoreInternetAccessStatus = false)
        {
            if (!ignoreInternetAccessStatus && !networkInfoService.InternetAccess) { return; }

            try
            {
                feed.StartLoading();
                if (feed.Uri.IsAbsoluteUri && (feed.Uri.Scheme == "http" || feed.Uri.Scheme == "https"))
                {
                    selectionService.SetDefaultSelectedFeedItem(feed, feed.Items.FirstOrDefault());
                    if (!selectionService.SelectedFeeds.Any())
                    {
                        selectionService.SelectFeed(feed);
                    }
                    try
                    {
                        var syndicationFeed = await client.RetrieveFeedAsync(feed.Uri);
                        var items = syndicationFeed.Items.Select(x => new FeedItem(x.Uri, x.Date, x.Name, x.Description)).ToArray();

                        feed.Name = syndicationFeed.Title;
                        feed.UpdateItems(items, excludeMarkAsRead: true);
                    }
                    catch (SyndicationServiceException ex) when (ex.Error == SyndicationServiceError.NotModified)
                    {
                        // Ignore the NotModified status
                    }
                }
                else
                {
                    feed.SetLoadError(new InvalidOperationException(@"The URL must begin with http:// or https://"), resourceService.GetString("UrlMustBeginWithHttp"));
                }
            }
            catch (Exception ex)
            {
                feed.SetLoadError(ex, resourceService.GetString("ErrorLoadRssFeed"));
            }
            finally
            {
                if (selectionService.SelectedFeedItem == null && selectionService.SelectedFeeds.FirstOrDefault() == feed)
                {
                    selectionService.SelectFeedItem(feed.Items.FirstOrDefault());
                }
            }
        }

        private async Task AddNewFeed()
        {
            Uri feedUri;
            var uriString = feedListViewModel.Value.AddNewFeedUri.Trim();
            if (!uriString.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
            {
                uriString = "http://" + uriString;
            }
            else if (uriString.StartsWith("http://http://", StringComparison.CurrentCultureIgnoreCase)
                || uriString.StartsWith("http://https://", StringComparison.CurrentCultureIgnoreCase))
            {
                uriString = uriString.Substring(7);
            }
            if (Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out feedUri))
            {
                var newFeed = new Feed(feedUri);
                await LoadFeedAsync(newFeed, ignoreInternetAccessStatus: true);
                feedListViewModel.Value.LoadErrorMessage = newFeed.LoadErrorMessage;
                if (newFeed.LoadError == null)
                {
                    FeedManager.Feeds.Add(newFeed);
                    selectionService.SelectFeed(newFeed);
                    feedListViewModel.Value.FeedAdded();
                }
            }
            else
            {
                feedListViewModel.Value.LoadErrorMessage = resourceService.GetString("UrlMustBeginWithHttp");
            }
        }

        private bool CanRemoveFeed(object parameter)
        {
            return parameter is Feed || selectionService.SelectedFeeds.Any();
        }

        private async Task RemoveFeedAsync(object parameter)
        {
            var feedParameter = parameter as Feed;
            var feedsToRemove = feedParameter != null ? new[] { feedParameter } : (IReadOnlyList<Feed>)selectionService.SelectedFeeds;
            if (!await messageService.ShowYesNoQuestionDialogAsync(resourceService.GetString("RemoveFeedQuestion"), string.Join(Environment.NewLine, feedsToRemove.Select(x => x.Name))))
            {
                return;  // User canceled operation
            }
            
            var feedToSelect = CollectionHelper.GetNextElementOrDefault(FeedManager.Feeds.Except(feedsToRemove.Except(new[] { feedsToRemove.First() })), feedsToRemove.First());
            foreach (var feed in feedsToRemove.ToArray()) FeedManager.Feeds.Remove(feed);
            selectionService.SelectFeed(feedToSelect ?? FeedManager.Feeds.LastOrDefault());
        }

        private async Task RefreshFeed()
        {
            await Task.WhenAll(FeedManager.Feeds.Select(x => LoadFeedAsync(x)));
        }

        private bool CanMarkAsReadUnread(object parameter)
        {
            return parameter is FeedItem || selectionService.SelectedFeedItem != null;
        }

        private void MarkAsReadUnread(object parameter)
        {
            var feedItem = parameter as FeedItem;
            var items = feedItem != null ? new[] { feedItem } : (IReadOnlyList<FeedItem>)selectionService.SelectedFeedItems;

            var stringParameter = parameter as string;
            if (string.Equals("read", stringParameter, StringComparison.OrdinalIgnoreCase)
                || (string.IsNullOrEmpty(stringParameter) && items.Any(x => !x.MarkAsRead)))
            {
                foreach (var item in items) item.MarkAsRead = true;
            }
            else
            {
                foreach (var item in items) item.MarkAsRead = false;
            }
        }

        private bool CanLaunchWebBrowser()
        {
            return selectionService.SelectedFeedItem != null;
        }

        private async Task LaunchWebBrowser()
        {
            await launcherService.LaunchUriAsync(selectionService.SelectedFeedItem.Uri);
        }

        private void SelectionServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(selectionService.SelectedFeedItem))
            {
                readUnreadCommand.RaiseCanExecuteChanged();
                launchWebBrowserCommand.RaiseCanExecuteChanged();
            }
        }

        private void SelectedFeedsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            removeFeedCommand.RaiseCanExecuteChanged();
        }
    }
}
