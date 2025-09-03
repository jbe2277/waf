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
        addFeedNavigationItem = new(Resources.AddFeed, "\uf412", "AddFeedItem") { Command = feedsController.AddFeedCommand };
        settingsNavigationItem = new(Resources.Settings, "\uf493", "SettingsItem")
        {
            Command = new DelegateCommand(() => shellViewModel.Navigate(this.settingsController.SettingsViewModel))
        };
        shellViewModel.ShowFeedViewCommand = feedsController.ShowFeedViewCommand;
        shellViewModel.EditFeedCommand = feedsController.EditFeedCommand;
        shellViewModel.RemoveFeedCommand = feedsController.RemoveFeedCommand;
        shellViewModel.FooterMenu = [ addFeedNavigationItem, settingsNavigationItem ];
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
        dataController.Save().GetAwaiter().GetResult();
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
                shellViewModel.SetSelectedFooterMenuCore(addFeedNavigationItem);
            }
            else if (shellViewModel.CurrentPage is ISettingsView)
            {
                shellViewModel.SetSelectedFooterMenuCore(settingsNavigationItem);
            }
            else if (shellViewModel.CurrentPage is IFeedView feedView)
            {
                shellViewModel.SetSelectedFeedCore(((FeedViewModel)feedView.BindingContext!).Feed);
            }
        }
    }
}
