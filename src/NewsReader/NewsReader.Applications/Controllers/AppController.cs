using System.Waf.Applications;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers;

internal sealed class AppController : IAppController
{
    private readonly INetworkInfoService networkInfoService;
    private readonly DataController dataController;
    private readonly SettingsController settingsController;
    private readonly FeedsController feedsController;
    private readonly ShellViewModel shellViewModel;
    private readonly NavigationItem addFeedNavigationItem;
    private readonly NavigationItem settingsNavigationItem;
    private FeedManager feedManager = null!;
    private DateTime lastUpdate;

    public AppController(INetworkInfoService networkInfoService, DataController dataController, FeedsController feedsController,
        SettingsController settingsController, ShellViewModel shellViewModel)
    {
        this.networkInfoService = networkInfoService;
        this.dataController = dataController;
        this.feedsController = feedsController;
        this.settingsController = settingsController;
        this.shellViewModel = shellViewModel;
        addFeedNavigationItem = new NavigationItem(Resources.AddFeed, "\uf412") { Command = feedsController.AddFeedCommand };
        settingsNavigationItem = new NavigationItem(Resources.Settings, "\uf493")
        {
            Command = new DelegateCommand(() => shellViewModel.Navigate(this.settingsController.SettingsViewModel))
        };
        shellViewModel.ShowFeedViewCommand = feedsController.ShowFeedViewCommand;
        shellViewModel.EditFeedCommand = feedsController.EditFeedCommand;
        shellViewModel.RemoveFeedCommand = feedsController.RemoveFeedCommand;
        shellViewModel.FooterMenu = new[] { addFeedNavigationItem, settingsNavigationItem };
        shellViewModel.PropertyChanged += ShellViewModelPropertyChanged;
        shellViewModel.Initialize();
        MainView = shellViewModel.View;
    }

    public object MainView { get; }

    public async void Start()
    {
        dataController.Initialize();
        feedManager = await dataController.Load();
        shellViewModel.Feeds = feedManager.Feeds;
        
        settingsController.FeedManager = feedManager;
        feedsController.FeedManager = feedManager;
        feedsController.Run();
        
        if (networkInfoService.InternetAccess) lastUpdate = DateTime.Now;
        networkInfoService.PropertyChanged += NetworkInfoServicePropertyChanged;
    }

    public void Save()
    {
        Task.Run(dataController.Save).GetAwaiter().GetResult();  // Task.Run needed to avoid dead-lock when Save uses await.
    }

    public async void Update()
    {
        if (networkInfoService.InternetAccess && DateTime.Now - lastUpdate > TimeSpan.FromMinutes(1))
        {
            lastUpdate = DateTime.Now;
            await dataController.Update();
            await feedsController.Update();
        }
    }

    private void NetworkInfoServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(networkInfoService.InternetAccess) && networkInfoService.InternetAccess) Update();
    }

    private void ShellViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ShellViewModel.CurrentPage))
        {
            if (shellViewModel.CurrentPage is IAddEditFeedView)
            {
                shellViewModel.SelectedFeed = null;
                shellViewModel.SelectedFooterMenu = addFeedNavigationItem;
            }
            else if (shellViewModel.CurrentPage is ISettingsView)
            {
                shellViewModel.SelectedFeed = null;
                shellViewModel.SelectedFooterMenu = settingsNavigationItem;
            }
            else if (shellViewModel.CurrentPage is IFeedView feedView)
            {
                shellViewModel.SelectedFeed = ((FeedViewModel)feedView.BindingContext!).Feed;
                shellViewModel.SelectedFooterMenu = null;
            }
        }
    }
}
