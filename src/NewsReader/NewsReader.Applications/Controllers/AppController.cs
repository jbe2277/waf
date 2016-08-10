using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.System;

namespace Jbe.NewsReader.Applications.Controllers
{
    [Export(typeof(IAppController)), Shared]
    internal class AppController : IAppController
    {
        private readonly Lazy<NewsFeedsController> newsFeedsController;
        private readonly Lazy<SettingsController> settingsController;
        private readonly SelectionService selectionService;
        private readonly Lazy<ShellViewModel> shellViewModel;
        private readonly Lazy<FeedListViewModel> feedListViewModel;
        private readonly Lazy<FeedItemListViewModel> feedItemListViewModel;
        private readonly Lazy<FeedItemViewModel> feedItemViewModel;
        private readonly DelegateCommand navigateBackCommand;
        private readonly DelegateCommand showFeedListViewCommand;
        private readonly DelegateCommand showFeedItemListViewCommand;
        private readonly DelegateCommand showFeedItemViewCommand;
        private readonly AsyncDelegateCommand showReviewViewCommand;
        private readonly DelegateCommand showSettingsViewCommand;
        private readonly AsyncDelegateCommand addNewFeedCommand;
        private readonly DelegateCommand removeFeedCommand;
        private readonly AsyncDelegateCommand refreshFeedCommand;
        private readonly DelegateCommand readUnreadCommand;
        private readonly AsyncDelegateCommand launchWebBrowserCommand;
        private readonly Stack<NavigationItem> navigationStack;
        private NavigationItem selectedNavigationItem;
        private bool isNavigatingBack;
        private FeedManager feedManager;


        [ImportingConstructor]
        public AppController(Lazy<NewsFeedsController> newsFeedsController, Lazy<SettingsController> settingsController,
            SelectionService selectionService, Lazy<ShellViewModel> shellViewModel,
            Lazy<FeedListViewModel> feedListViewModel, Lazy<FeedItemListViewModel> feedItemListViewModel, Lazy<FeedItemViewModel> feedItemViewModel)
        {
            this.newsFeedsController = newsFeedsController;
            this.settingsController = settingsController;
            this.selectionService = selectionService;
            this.shellViewModel = shellViewModel;
            this.feedListViewModel = new Lazy<FeedListViewModel>(() => InitializeFeedListViewModel(feedListViewModel));
            this.feedItemListViewModel = new Lazy<FeedItemListViewModel>(() => InitializeFeedItemListViewModel(feedItemListViewModel));
            this.feedItemViewModel = new Lazy<FeedItemViewModel>(() => InitializeFeedItemViewModel(feedItemViewModel));
            this.navigateBackCommand = new DelegateCommand(NavigateBack, CanNavigateBack);
            this.showFeedListViewCommand = new DelegateCommand(() => SelectedNavigationItem = NavigationItem.FeedList);
            this.showFeedItemListViewCommand = new DelegateCommand(ShowFeedItemListView);
            this.showFeedItemViewCommand = new DelegateCommand(ShowFeedItemView);
            this.showReviewViewCommand = new AsyncDelegateCommand(ShowReviewView);
            this.showSettingsViewCommand = new DelegateCommand(ShowSettingsView);
            this.addNewFeedCommand = new AsyncDelegateCommand(AddNewFeed);
            this.removeFeedCommand = new DelegateCommand(RemoveFeed, CanRemoveFeed);
            this.refreshFeedCommand = new AsyncDelegateCommand(RefreshFeed, CanRefreshFeed);
            this.readUnreadCommand = new DelegateCommand(MarkAsReadUnread, CanMarkAsReadUnread);
            this.launchWebBrowserCommand = new AsyncDelegateCommand(LaunchWebBrowser, CanLaunchWebBrowser);
            this.navigationStack = new Stack<NavigationItem>();

            this.selectionService.PropertyChanged += SelectionServicePropertyChanged;
        }

        private NavigationItem SelectedNavigationItem
        {
            get { return selectedNavigationItem; }
            set
            {
                if (selectedNavigationItem != value)
                {
                    if (!isNavigatingBack)
                    {
                        navigationStack.Push(selectedNavigationItem);
                        navigateBackCommand.RaiseCanExecuteChanged();
                    }
                    selectedNavigationItem = value;
                    shellViewModel.Value.SelectedNavigationItem = value;
                    Navigate();
                }
            }
        }


        public void Initialize()
        {
            shellViewModel.Value.NavigateBackCommand = navigateBackCommand;
            shellViewModel.Value.ShowNewsViewCommand = showFeedListViewCommand;
            shellViewModel.Value.ShowReviewViewCommand = showReviewViewCommand;
            shellViewModel.Value.ShowSettingsViewCommand = showSettingsViewCommand;
        }

        public async void Run()
        {
            Navigate();
            shellViewModel.Value.Show();

            try
            {
                await FileIOHelper.MigrateDataAsync(ApplicationData.Current.RoamingFolder, "feeds.xml");
                feedManager = await FileIOHelper.LoadCompressedAsync<FeedManager>(ApplicationData.Current.RoamingFolder, "feeds.xml") ?? new FeedManager();
            }
            catch (Exception ex)
            {
                // Better to forget the settings (data loss) as to never start the app again
                Debug.Assert(false, "LoadAsync", ex.ToString());
                feedManager = new FeedManager();
            }
            selectionService.FeedManager = feedManager;
            newsFeedsController.Value.FeedManager = feedManager;
            newsFeedsController.Value.Run();
            settingsController.Value.FeedManager = feedManager;
            if (feedListViewModel.IsValueCreated) { feedListViewModel.Value.FeedManager = feedManager; }
        }

        public async Task SuspendingAsync()
        {
            await FileIOHelper.SaveCompressedAsync(feedManager, ApplicationData.Current.RoamingFolder, "feeds.xml");
        }

        private FeedListViewModel InitializeFeedListViewModel(Lazy<FeedListViewModel> viewModel)
        {
            viewModel.Value.FeedManager = feedManager;
            viewModel.Value.AddNewFeedCommand = addNewFeedCommand;
            viewModel.Value.RemoveFeedCommand = removeFeedCommand;
            viewModel.Value.ShowFeedItemListViewCommand = showFeedItemListViewCommand;
            return viewModel.Value;
        }

        private FeedItemListViewModel InitializeFeedItemListViewModel(Lazy<FeedItemListViewModel> viewModel)
        {
            viewModel.Value.RefreshCommand = refreshFeedCommand;
            viewModel.Value.ReadUnreadCommand = readUnreadCommand;
            viewModel.Value.ShowFeedItemViewCommand = showFeedItemViewCommand;
            return viewModel.Value;
        }

        private FeedItemViewModel InitializeFeedItemViewModel(Lazy<FeedItemViewModel> viewModel)
        {
            viewModel.Value.LaunchWebBrowserCommand = launchWebBrowserCommand;
            return viewModel.Value;
        }
        
        private void Navigate()
        {
            // First we need to set them to null so that not the same view is set in both properties => exception
            shellViewModel.Value.ContentView = null;
            shellViewModel.Value.LazyPreviewView = null;
            switch (SelectedNavigationItem)
            {
                case NavigationItem.FeedList:
                    shellViewModel.Value.ContentView = feedListViewModel.Value.View;
                    shellViewModel.Value.LazyPreviewView = new Lazy<object>(() => feedItemListViewModel.Value.View);
                    break;
                case NavigationItem.FeedItemList:
                    shellViewModel.Value.ContentView = feedItemListViewModel.Value.View;
                    shellViewModel.Value.LazyPreviewView = new Lazy<object>(() => feedItemViewModel.Value.View);
                    break;
                case NavigationItem.FeedItem:
                    shellViewModel.Value.ContentView = feedItemViewModel.Value.View;
                    break;
                case NavigationItem.Settings:
                    shellViewModel.Value.ContentView = settingsController.Value.SettingsView;
                    break;
            }
        }

        private bool CanNavigateBack()
        {
            return navigationStack.Any();
        }

        private void NavigateBack()
        {
            isNavigatingBack = true;
            SelectedNavigationItem = navigationStack.Pop();
            isNavigatingBack = false;
            navigateBackCommand.RaiseCanExecuteChanged();
        }

        private void ShowFeedItemListView(object parameter)
        {
            selectionService.SelectedFeed = (Feed)parameter;
            SelectedNavigationItem = NavigationItem.FeedItemList;
        }

        private void ShowFeedItemView(object parameter)
        {
            selectionService.SelectedFeedItem = (FeedItem)parameter;
            if (SelectedNavigationItem == NavigationItem.FeedList)
            {
                SelectedNavigationItem = NavigationItem.FeedItemList;
            }
            else
            {
                SelectedNavigationItem = NavigationItem.FeedItem;
            }
        }

        private async Task ShowReviewView()
        {
            // https://msdn.microsoft.com/en-us/library/windows/apps/mt228343.aspx
            await Launcher.LaunchUriAsync(new Uri(string.Format(CultureInfo.InvariantCulture, "ms-windows-store:review?PFN={0}", Package.Current.Id.FamilyName)));
        }

        private void ShowSettingsView()
        {
            SelectedNavigationItem = NavigationItem.Settings;
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
                await newsFeedsController.Value.LoadFeedAsync(newFeed);
                feedListViewModel.Value.LoadErrorMessage = newFeed.LoadErrorMessage;
                if (newFeed.LoadError == null)
                {
                    feedManager.Feeds.Add(newFeed);
                    selectionService.SelectedFeed = newFeed;
                    feedListViewModel.Value.FeedAdded();
                }
            }
            else
            {   
                feedListViewModel.Value.LoadErrorMessage = ResourceLoader.GetForViewIndependentUse().GetString("UrlMustBeginWithHttp");
            }
        }

        private bool CanRemoveFeed()
        {
            return selectionService.SelectedFeed != null;
        }

        private void RemoveFeed()
        {
            var feedToRemove = selectionService.SelectedFeed;
            var feedToSelect = CollectionHelper.GetNextElementOrDefault(feedManager.Feeds, feedToRemove);
            feedManager.Feeds.Remove(feedToRemove);
            selectionService.SelectedFeed = feedToSelect ?? feedManager.Feeds.LastOrDefault();
        }

        private bool CanRefreshFeed()
        {
            return selectionService.SelectedFeed != null;
        }

        private async Task RefreshFeed()
        {
            await newsFeedsController.Value.LoadFeedAsync(selectionService.SelectedFeed);
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
            await Launcher.LaunchUriAsync(selectionService.SelectedFeedItem.Uri);
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
