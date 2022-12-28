using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers;

internal class AppController : IAppController
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
        shellViewModel.MoveFeedUpCommand = feedsController.MoveFeedUpCommand;
        shellViewModel.MoveFeedDownCommand = feedsController.MoveFeedDownCommand;
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

    public void Resume()
    {
        if (networkInfoService.InternetAccess && DateTime.Now - lastUpdate > TimeSpan.FromMinutes(1))
        {
            dataController.Update().NoWait();
            feedsController.Update().NoWait();
            lastUpdate = DateTime.Now;
        }
    }

    private void NetworkInfoServicePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(networkInfoService.InternetAccess) && networkInfoService.InternetAccess)
        {
            Resume();
        }
    }
}
