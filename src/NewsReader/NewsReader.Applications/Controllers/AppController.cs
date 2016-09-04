using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Domain;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Waf.Applications;
using Windows.Storage;

namespace Jbe.NewsReader.Applications.Controllers
{
    [Export(typeof(IAppController)), Shared]
    internal class AppController : IAppController
    {
        private readonly ILauncherService launcherService;
        private readonly IAppInfoService appInfoService;
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
        private readonly Stack<NavigationItem> navigationStack;
        private NavigationItem selectedNavigationItem;
        private bool isNavigatingBack;
        private FeedManager feedManager;


        [ImportingConstructor]
        public AppController(ILauncherService launcherService, IAppInfoService appInfoService, Lazy<NewsFeedsController> newsFeedsController, Lazy<SettingsController> settingsController,
            SelectionService selectionService, Lazy<ShellViewModel> shellViewModel,
            Lazy<FeedListViewModel> feedListViewModel, Lazy<FeedItemListViewModel> feedItemListViewModel, Lazy<FeedItemViewModel> feedItemViewModel)
        {
            this.launcherService = launcherService;
            this.appInfoService = appInfoService;
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
            this.navigationStack = new Stack<NavigationItem>();
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
            viewModel.Value.AddNewFeedCommand = newsFeedsController.Value.AddNewFeedCommand;
            viewModel.Value.RemoveFeedCommand = newsFeedsController.Value.RemoveFeedCommand;
            viewModel.Value.ShowFeedItemListViewCommand = showFeedItemListViewCommand;
            return viewModel.Value;
        }

        private FeedItemListViewModel InitializeFeedItemListViewModel(Lazy<FeedItemListViewModel> viewModel)
        {
            viewModel.Value.RefreshCommand = newsFeedsController.Value.RefreshFeedCommand;
            viewModel.Value.ReadUnreadCommand = newsFeedsController.Value.ReadUnreadCommand;
            viewModel.Value.ShowFeedItemViewCommand = showFeedItemViewCommand;
            return viewModel.Value;
        }

        private FeedItemViewModel InitializeFeedItemViewModel(Lazy<FeedItemViewModel> viewModel)
        {
            viewModel.Value.LaunchWebBrowserCommand = newsFeedsController.Value.LaunchWebBrowserCommand;
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

        private Task ShowReviewView()
        {
            return launcherService.LaunchReviewAsync();
        }

        private void ShowSettingsView()
        {
            SelectedNavigationItem = NavigationItem.Settings;
        }
    }
}
