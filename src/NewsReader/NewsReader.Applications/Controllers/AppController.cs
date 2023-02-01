using System.Waf.Applications;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers;

internal sealed class AppController : IAppController
{
    private readonly INetworkInfoService networkInfoService;
    private readonly DataController dataController;
    private readonly SettingsController settingsController;
    private readonly FeedsController feedsController;
    private readonly ShellViewModel shellViewModel;
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
        shellViewModel.ShowFeedViewCommand = feedsController.ShowFeedViewCommand;
        shellViewModel.EditFeedCommand = feedsController.EditFeedCommand;
        shellViewModel.RemoveFeedCommand = feedsController.RemoveFeedCommand;
        shellViewModel.FooterMenu = new[]
        {
            new NavigationItem("Add Feed", "\uf412") { Command = feedsController.AddFeedCommand },
            new NavigationItem("Settings", "\uf493") { Command = new AsyncDelegateCommand(() =>
                shellViewModel.Navigate(this.settingsController.SettingsViewModel)) }
        };
        shellViewModel.Initialize();
        MainView = shellViewModel.View;
    }

    public object MainView { get; }

    public async void Start()
    {
        dataController.Initialize();
        feedManager = await dataController.Load();
        settingsController.FeedManager = feedManager;
        feedsController.FeedManager = feedManager;
        feedsController.Run();
        if (networkInfoService.InternetAccess) { lastUpdate = DateTime.Now; }
        networkInfoService.PropertyChanged += NetworkInfoServicePropertyChanged;

        shellViewModel.Feeds = feedManager.Feeds;
    }

    public void Sleep()
    {
        Task.Run(dataController.Save).GetAwaiter().GetResult();  // Task.Run needed to avoid dead-lock when Save uses await.
    }

    public async void Resume()
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
        if (e.PropertyName is nameof(networkInfoService.InternetAccess) && networkInfoService.InternetAccess) Resume();
    }
}
