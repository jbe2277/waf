using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers;

internal sealed class FeedsController
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
    private readonly DelegateCommand addFeedCommand;
    private readonly AsyncDelegateCommand addEditLoadFeedCommand;
    private readonly AsyncDelegateCommand addUpdateFeedCommand;
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
        this.addEditFeedViewModel = new(() => InitializeViewModel(addEditFeedViewModel.Value));
        this.feedViewModel = new(() => InitializeViewModel(feedViewModel.Value));
        this.feedItemViewModel = new(() => InitializeViewModel(feedItemViewModel.Value));
        addFeedCommand = new(AddFeed);
        addEditLoadFeedCommand = new(AddEditLoadFeed);
        addUpdateFeedCommand = new(AddUpdateFeed, CanAddUpdateFeed);
        showFeedViewCommand = new(ShowFeedView);
        editFeedCommand = new(EditFeed);
        removeFeedCommand = new(RemoveFeed);
        refreshFeedCommand = new(Update);
        readUnreadCommand = new(MarkAsReadUnread);
        showFeedItemViewCommand = new(ShowFeedItemView);
        launchBrowserCommand = new(LaunchBrowser, CanLaunchBrowser);
    }

    public FeedManager FeedManager { get; set; } = null!;

    public ICommand AddFeedCommand => addFeedCommand;

    public ICommand ShowFeedViewCommand => showFeedViewCommand;

    public ICommand EditFeedCommand => editFeedCommand;

    public ICommand RemoveFeedCommand => removeFeedCommand;

    private AddEditFeedViewModel AddEditFeedViewModel => addEditFeedViewModel.Value;

    public async void Run()
    {
        FeedManager.Feeds.CollectionChanging += FeedsCollectionChanging;
        FeedManager.Feeds.CollectionChanged += FeedsCollectionChanged;
        await ShowFeedView(FeedManager.Feeds.FirstOrDefault());
        await Update();
    }

    public Task Update() => Task.WhenAll(FeedManager.Feeds.Select(x => LoadFeed(x)));

    private void FeedsCollectionChanging(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var oldItems = e.Action == NotifyCollectionChangedAction.Reset ? FeedManager.Feeds : e.OldItems?.Cast<Feed>();
        if (oldItems?.Any(x => x == feedViewModel.Value.Feed) == true)
        {
            shellViewModel.SelectedFeed = feedViewModel.Value.Feed = null;
        }
    }

    private void FeedsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (Feed x in e.NewItems ?? Array.Empty<Feed>()) LoadFeed(x).NoWait();
    }

    private async Task LoadFeed(Feed feed, bool ignoreInternetAccessStatus = false)
    {
        if (!ignoreInternetAccessStatus && !networkInfoService.InternetAccess) return;
        if (feed.IsLoading) return;
        try
        {
            feed.StartLoading();
            if (feed.Uri.IsAbsoluteUri && (feed.Uri.Scheme == "http" || feed.Uri.Scheme == "https"))
            {
                var syndicationFeed = await syndicationService.RetrieveFeed(feed.Uri);
                var items = syndicationFeed.Items.Where(x => x.Uri != null).Select(x => new FeedItem(x.Uri!, x.Date, x.Name, x.Description)).ToArray();
                feed.Title = syndicationFeed.Title;
                feed.UpdateItems(items, excludeMarkAsRead: true);
            }
            else
            {
                feed.SetLoadError(new InvalidOperationException(@"The URL must begin with http:// or https://"), Resources.UrlMustBeginWithHttp);
            }
        }
        catch (Exception ex)
        {
            Log.Default.TrackError(ex, "Load Feed failed");
            feed.SetLoadError(ex, Resources.ErrorLoadRssFeed + " " + ex.Message);
        }
    }

    private void AddFeed()
    {
        AddEditFeedViewModel.IsEditMode = false;
        AddEditFeedViewModel.FeedUrl = null;
        AddEditFeedViewModel.OldFeed = null;
        AddEditFeedViewModel.Feed = null;
        navigationService.Navigate(AddEditFeedViewModel);
    }

    private async Task AddEditLoadFeed()
    {
        if (AddEditFeedViewModel.Feed?.Uri.ToString() == AddEditFeedViewModel.FeedUrl) return;
        var uriString = AddEditFeedViewModel.FeedUrl?.Trim() ?? "";
        if (!uriString.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
        {
            uriString = "http://" + uriString;
        }
        else if (uriString.StartsWith("http://http://", StringComparison.CurrentCultureIgnoreCase)
            || uriString.StartsWith("http://https://", StringComparison.CurrentCultureIgnoreCase))
        {
            uriString = uriString[7..];
        }
        if (Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out var feedUri))
        {
            var newFeed = new Feed(feedUri);
            AddEditFeedViewModel.Feed = newFeed;
            await LoadFeed(newFeed, ignoreInternetAccessStatus: true);
            AddEditFeedViewModel.LoadErrorMessage = newFeed.LoadErrorMessage;
        }
        else
        {
            AddEditFeedViewModel.LoadErrorMessage = Resources.UrlMustBeginWithHttp;
        }
    }

    private bool CanAddUpdateFeed()
    {
        return AddEditFeedViewModel.Feed?.HasErrors == false && AddEditFeedViewModel.Feed?.IsLoading == false
            && string.IsNullOrEmpty(AddEditFeedViewModel.LoadErrorMessage);
    }

    private async Task AddUpdateFeed()
    {
        if (AddEditFeedViewModel.IsEditMode)
        {
            if (AddEditFeedViewModel.OldFeed != AddEditFeedViewModel.Feed)
            {
                var index = FeedManager.Feeds.IndexOf(AddEditFeedViewModel.OldFeed!);
                FeedManager.Feeds[index] = AddEditFeedViewModel.Feed!;
            }
        }
        else
        {
            FeedManager.Feeds.Add(AddEditFeedViewModel.Feed!);
        }
        shellViewModel.SelectedFeed = feedViewModel.Value.Feed = AddEditFeedViewModel.Feed;
        await navigationService.NavigateBack();
    }

    private Task ShowFeedView(object? parameter)
    {
        shellViewModel.SelectedFeed = feedViewModel.Value.Feed = (Feed?)parameter;
        return navigationService.Navigate(feedViewModel.Value);
    }

    private Task ShowFeedItemView(object? parameter)
    {
        feedItemViewModel.Value.FeedItem = (FeedItem?)parameter;
        return navigationService.Navigate(feedItemViewModel.Value);
    }

    private Task EditFeed(object? parameter)
    {
        var feed = (Feed)(parameter ?? throw new InvalidOperationException("parameter of the edit command must not be null"));
        shellViewModel.SelectedFeed = feedViewModel.Value.Feed = feed;
        AddEditFeedViewModel.IsEditMode = true;
        AddEditFeedViewModel.FeedUrl = feed.Uri.ToString();
        AddEditFeedViewModel.OldFeed = feed;
        AddEditFeedViewModel.Feed = feed;
        return navigationService.Navigate(AddEditFeedViewModel);
    }

    private async Task RemoveFeed(object? parameter)
    {
        var feed = (Feed)(parameter ?? throw new InvalidOperationException("parameter of the remove command must not be null"));
        if (await messageService.ShowYesNoQuestion(Resources.RemoveFeedQuestion, feed.Name))
        {
            FeedManager.Feeds.Remove(feed);
        }
    }

    private void MarkAsReadUnread(object? parameter)
    {
        var feedItem = (FeedItem)parameter!;
        feedItem.MarkAsRead = !feedItem.MarkAsRead;
    }

    private bool CanLaunchBrowser() => feedItemViewModel.Value.FeedItem != null;

    private async Task LaunchBrowser() => await launcherService.LaunchBrowser(feedItemViewModel.Value.FeedItem!.Uri);

    private AddEditFeedViewModel InitializeViewModel(AddEditFeedViewModel vm)
    {
        vm.LoadFeedCommand = addEditLoadFeedCommand;
        vm.AddUpdateCommand = addUpdateFeedCommand;
        vm.PropertyChanged += AddEditFeedViewModelPropertyChanged;
        vm.FeedChanged += AddEditFeedViewModelFeedChanged;
        return vm;
    }

    private FeedViewModel InitializeViewModel(FeedViewModel vm)
    {
        vm.RefreshCommand = refreshFeedCommand;
        vm.ReadUnreadCommand = readUnreadCommand;
        vm.ShowFeedItemViewCommand = showFeedItemViewCommand;
        vm.PropertyChanged += FeedViewModelPropertyChanged;
        return vm;
    }

    private FeedItemViewModel InitializeViewModel(FeedItemViewModel vm)
    {
        vm.LaunchBrowserCommand = launchBrowserCommand;
        vm.PropertyChanged += FeedItemViewModelPropertyChanged;
        return vm;
    }

    private void AddEditFeedViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(AddEditFeedViewModel.LoadErrorMessage) or nameof(AddEditFeedViewModel.Feed)) addUpdateFeedCommand.RaiseCanExecuteChanged();
    }

    private void AddEditFeedViewModelFeedChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(Feed.HasErrors) or nameof(Feed.IsLoading)) addUpdateFeedCommand.RaiseCanExecuteChanged();
    }

    private void FeedViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(FeedViewModel.Feed))
        {
            if (feedItemViewModel.IsValueCreated) feedItemViewModel.Value.FeedItem = null;
        }
    }

    private void FeedItemViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(FeedItemViewModel.FeedItem)) launchBrowserCommand.RaiseCanExecuteChanged();
    }
}
