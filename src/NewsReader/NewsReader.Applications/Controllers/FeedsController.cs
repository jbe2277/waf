using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers
{
    internal class FeedsController
    {
        private readonly IMessageService messageService;
        private readonly INavigationService navigationService;
        private readonly ISyndicationService syndicationService;
        private readonly INetworkInfoService networkInfoService;
        private readonly ILauncherService launcherService;
        private readonly ShellViewModel shellViewModel;
        private readonly Lazy<AddEditFeedViewModel> addEditFeedViewModel;
        private readonly Lazy<FeedViewModel> feedViewModel;
        private readonly Lazy<FeedItemViewModel> feedItemViewModel;
        private readonly AsyncDelegateCommand addFeedCommand;
        private readonly AsyncDelegateCommand showFeedViewCommand;
        private readonly AsyncDelegateCommand editFeedCommand;
        private readonly AsyncDelegateCommand removeFeedCommand;
        private readonly AsyncDelegateCommand refreshFeedCommand;
        private readonly DelegateCommand readUnreadCommand;
        private readonly AsyncDelegateCommand showFeedItemViewCommand;
        private readonly AsyncDelegateCommand launchBrowserCommand;

        public FeedsController(IMessageService messageService, INavigationService navigationService, ISyndicationService syndicationService, 
            INetworkInfoService networkInfoService, ILauncherService launcherService, ShellViewModel shellViewModel, 
            Lazy<AddEditFeedViewModel> addEditFeedViewModel, Lazy<FeedViewModel> feedViewModel, Lazy<FeedItemViewModel> feedItemViewModel)
        {
            this.messageService = messageService;
            this.navigationService = navigationService;
            this.syndicationService = syndicationService;
            this.networkInfoService = networkInfoService;
            this.launcherService = launcherService;
            this.shellViewModel = shellViewModel;
            this.addEditFeedViewModel = new Lazy<AddEditFeedViewModel>(() => InitializeViewModel(addEditFeedViewModel.Value));
            this.feedViewModel = new Lazy<FeedViewModel>(() => InitializeViewModel(feedViewModel.Value));
            this.feedItemViewModel = new Lazy<FeedItemViewModel>(() => InitializeViewModel(feedItemViewModel.Value));
            addFeedCommand = new AsyncDelegateCommand(AddFeed);
            showFeedViewCommand = new AsyncDelegateCommand(ShowFeedView);
            editFeedCommand = new AsyncDelegateCommand(EditFeed);
            removeFeedCommand = new AsyncDelegateCommand(RemoveFeed);
            refreshFeedCommand = new AsyncDelegateCommand(RefreshFeed);
            readUnreadCommand = new DelegateCommand(MarkAsReadUnread);
            showFeedItemViewCommand = new AsyncDelegateCommand(ShowFeedItemView);
            launchBrowserCommand = new AsyncDelegateCommand(LaunchBrowser, CanLaunchBrowser);
        }

        public FeedManager FeedManager { get; set; }

        public ICommand AddFeedCommand => addFeedCommand;

        public ICommand ShowFeedViewCommand => showFeedViewCommand;

        public ICommand EditFeedCommand => editFeedCommand;

        public ICommand RemoveFeedCommand => removeFeedCommand;

        public void Run()
        {
            Update();
            FeedManager.Feeds.CollectionChanged += FeedsCollectionChanged;
            ShowFeedView(FeedManager.Feeds.FirstOrDefault()).NoWait();
        }

        public void Update()
        {
            foreach (var feed in FeedManager.Feeds)
            {
                LoadFeedAsync(feed).NoWait();
            }
        }

        private void FeedsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Feed item in e.NewItems?.Cast<Feed>() ?? Array.Empty<Feed>())
            {
                LoadFeedAsync(item).NoWait();
            }
            if ((e.OldItems?.Cast<Feed>() ?? Array.Empty<Feed>()).Any(x => x == feedViewModel.Value.Feed))
            {
                shellViewModel.SelectedFeed = feedViewModel.Value.Feed = null;
            }
        }

        private async Task LoadFeedAsync(Feed feed, bool ignoreInternetAccessStatus = false)
        {
            if (!ignoreInternetAccessStatus && !networkInfoService.InternetAccess) { return; }

            try
            {
                feed.StartLoading();
                if (feed.Uri.IsAbsoluteUri && (feed.Uri.Scheme == "http" || feed.Uri.Scheme == "https"))
                {
                    var syndicationFeed = await syndicationService.RetrieveFeed(feed.Uri);
                    var items = syndicationFeed.Items.Select(x => new FeedItem(x.Uri, x.Date, x.Name, x.Description)).ToArray();
                    feed.Name = syndicationFeed.Title;
                    feed.UpdateItems(items, excludeMarkAsRead: true);
                }
                else
                {
                    feed.SetLoadError(new InvalidOperationException(@"The URL must begin with http:// or https://"), Resources.UrlMustBeginWithHttp);
                }
            }
            catch (Exception ex)
            {
                Log.Default.Error(ex, "Load Feed failed.");
                Crashes.TrackError(ex);
                feed.SetLoadError(ex, Resources.ErrorLoadRssFeed);
            }
        }

        private Task AddFeed()
        {
            addEditFeedViewModel.Value.IsEditMode = false;
            addEditFeedViewModel.Value.FeedUrl = null;
            addEditFeedViewModel.Value.Feed = null;
            return navigationService.Navigate(addEditFeedViewModel.Value);
        }

        private Task ShowFeedView(object parameter)
        {
            shellViewModel.SelectedFeed = feedViewModel.Value.Feed = (Feed)parameter;
            return navigationService.Navigate(feedViewModel.Value);
        }

        private Task ShowFeedItemView(object parameter)
        {
            feedItemViewModel.Value.FeedItem = (FeedItem)parameter;
            return navigationService.Navigate(feedItemViewModel.Value);
        }

        private Task EditFeed(object parameter)
        {
            var feed = (Feed)parameter;
            shellViewModel.SelectedFeed = feedViewModel.Value.Feed = feed;
            addEditFeedViewModel.Value.IsEditMode = true;
            addEditFeedViewModel.Value.FeedUrl = feed.Uri.ToString();
            addEditFeedViewModel.Value.Feed = feed;
            return navigationService.Navigate(addEditFeedViewModel.Value);
        }

        private async Task RemoveFeed(object parameter)
        {
            var feed = (Feed)parameter;
            if (await messageService.ShowYesNoQuestion(Resources.RemoveFeedQuestion, feed.Name))
            {
                FeedManager.Feeds.Remove(feed);
            }
        }

        private async Task RefreshFeed()
        {
            await Task.WhenAll(FeedManager.Feeds.Select(x => LoadFeedAsync(x)));
        }

        private void MarkAsReadUnread(object parameter)
        {
            var feedItem = (FeedItem)parameter;
            feedItem.MarkAsRead = !feedItem.MarkAsRead;
        }

        private bool CanLaunchBrowser()
        {
            return feedItemViewModel.Value.FeedItem != null;
        }

        private async Task LaunchBrowser()
        {
            await launcherService.LaunchBrowser(feedItemViewModel.Value.FeedItem.Uri);
        }

        private AddEditFeedViewModel InitializeViewModel(AddEditFeedViewModel viewModel)
        {
            return viewModel;
        }

        private FeedViewModel InitializeViewModel(FeedViewModel viewModel)
        {
            viewModel.RefreshCommand = refreshFeedCommand;
            viewModel.ReadUnreadCommand = readUnreadCommand;
            viewModel.ShowFeedItemViewCommand = showFeedItemViewCommand;
            viewModel.PropertyChanged += FeedViewModelPropertyChanged;
            return viewModel;
        }

        private FeedItemViewModel InitializeViewModel(FeedItemViewModel viewModel)
        {
            viewModel.LaunchBrowserCommand = launchBrowserCommand;
            viewModel.PropertyChanged += FeedItemViewModelPropertyChanged;
            return viewModel;
        }

        private void FeedViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FeedViewModel.Feed))
            {
                if (feedItemViewModel.IsValueCreated) feedItemViewModel.Value.FeedItem = null;
            }
        }

        private void FeedItemViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FeedItemViewModel.FeedItem))
            {
                launchBrowserCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
