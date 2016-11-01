using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Domain;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Composition;
using System.Globalization;
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
            ISyndicationService syndicationService, SelectionService selectionService, Lazy<FeedListViewModel> feedListViewModel)
        {
            this.resourceService = resourceService;
            this.appService = appService;
            this.launcherService = launcherService;
            this.messageService = messageService;
            this.selectionService = selectionService;
            this.feedListViewModel = feedListViewModel;
            this.client = syndicationService.CreateClient();
            this.addNewFeedCommand = new AsyncDelegateCommand(AddNewFeed);
            this.removeFeedCommand = new AsyncDelegateCommand(RemoveFeedAsync, CanRemoveFeed);
            this.refreshFeedCommand = new AsyncDelegateCommand(RefreshFeed, CanRefreshFeed);
            this.readUnreadCommand = new DelegateCommand(MarkAsReadUnread, CanMarkAsReadUnread);
            this.launchWebBrowserCommand = new AsyncDelegateCommand(LaunchWebBrowser, CanLaunchWebBrowser);

            this.selectionService.PropertyChanged += SelectionServicePropertyChanged;
        }


        public FeedManager FeedManager { get; set; }

        public ICommand AddNewFeedCommand => addNewFeedCommand;

        public ICommand RemoveFeedCommand => removeFeedCommand;

        public ICommand RefreshFeedCommand => refreshFeedCommand;

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
            
            // Enforce scroll into view after loading more items
            var itemToSelectAgain = selectionService.SelectedFeedItem;
            selectionService.SelectedFeedItem = null;
            selectionService.SelectedFeedItem = itemToSelectAgain;

            await Task.WhenAll(tasks);
            FeedManager.Feeds.CollectionChanged += FeedsCollectionChanged;
        }

        private void FeedsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Feed item in e.NewItems?.Cast<Feed>() ?? CollectionHelper.Empty<Feed>())
            {
                IgnoreResult(LoadFeedAsync(item));
            }
        }

        private static void IgnoreResult(Task task) { }

        private async Task LoadFeedAsync(Feed feed)
        {
            try
            {
                if (feed.Uri.IsAbsoluteUri && (feed.Uri.Scheme == "http" || feed.Uri.Scheme == "https"))
                {
                    selectionService.SetDefaultSelectedFeedItem(feed, feed.Items.FirstOrDefault());
                    if (selectionService.SelectedFeed == null)
                    {
                        selectionService.SelectedFeed = feed;                        
                    }
                    var syndicationFeed = await client.RetrieveFeedAsync(feed.Uri);
                    var items = syndicationFeed.Items.Select(x => new FeedItem(x.Uri, x.Date, x.Name, x.Description, x.Author)).ToArray();

                    feed.Name = syndicationFeed.Title;
                    feed.UpdateItems(items);
                }
                else
                {
                    feed.LoadErrorMessage = resourceService.GetString("UrlMustBeginWithHttp");
                    feed.LoadError = new InvalidOperationException(@"The URL must begin with http:// or https://");
                }
            }
            catch (Exception ex)
            {
                feed.LoadErrorMessage = resourceService.GetString("ErrorLoadRssFeed");
                feed.LoadError = ex;
            }
            finally
            {
                if (selectionService.SelectedFeedItem == null && selectionService.SelectedFeed == feed)
                {
                    selectionService.SelectedFeedItem = feed.Items.FirstOrDefault();
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
                await LoadFeedAsync(newFeed);
                feedListViewModel.Value.LoadErrorMessage = newFeed.LoadErrorMessage;
                if (newFeed.LoadError == null)
                {
                    FeedManager.Feeds.Add(newFeed);
                    selectionService.SelectedFeed = newFeed;
                    feedListViewModel.Value.FeedAdded();
                }
            }
            else
            {
                feedListViewModel.Value.LoadErrorMessage = resourceService.GetString("UrlMustBeginWithHttp");
            }
        }

        private bool CanRemoveFeed()
        {
            return selectionService.SelectedFeed != null;
        }

        private async Task RemoveFeedAsync()
        {
            var feedToRemove = selectionService.SelectedFeed;
            if (!await messageService.ShowYesNoQuestionAsync(string.Format(CultureInfo.CurrentCulture, 
                resourceService.GetString("RemoveFeedQuestion"), feedToRemove.Name)))
            {
                return;  // User canceled operation
            }
            
            var feedToSelect = CollectionHelper.GetNextElementOrDefault(FeedManager.Feeds, feedToRemove);
            FeedManager.Feeds.Remove(feedToRemove);
            selectionService.SelectedFeed = feedToSelect ?? FeedManager.Feeds.LastOrDefault();
        }

        private bool CanRefreshFeed()
        {
            return selectionService.SelectedFeed != null;
        }

        private async Task RefreshFeed()
        {
            await LoadFeedAsync(selectionService.SelectedFeed);
        }

        private bool CanMarkAsReadUnread(object parameter)
        {
            return selectionService.SelectedFeedItem != null;
        }

        private void MarkAsReadUnread(object parameter)
        {
            var stringParameter = parameter as string;
            if (string.Equals("read", stringParameter, StringComparison.OrdinalIgnoreCase))
            {
                selectionService.SelectedFeedItem.MarkAsRead = true;
            }
            else if (string.Equals("unread", stringParameter, StringComparison.OrdinalIgnoreCase))
            {
                selectionService.SelectedFeedItem.MarkAsRead = false;
            }
            else
            {
                selectionService.SelectedFeedItem.MarkAsRead = !selectionService.SelectedFeedItem.MarkAsRead;
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
            if (e.PropertyName == nameof(selectionService.SelectedFeed))
            {
                removeFeedCommand.RaiseCanExecuteChanged();
                refreshFeedCommand.RaiseCanExecuteChanged();
            }
            if (e.PropertyName == nameof(selectionService.SelectedFeedItem))
            {
                readUnreadCommand.RaiseCanExecuteChanged();
                launchWebBrowserCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
