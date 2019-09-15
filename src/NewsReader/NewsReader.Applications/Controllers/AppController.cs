using System;
using System.Threading.Tasks;
using System.Waf.Applications;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers
{
    internal class AppController : IAppController
    {
        private readonly INetworkInfoService networkInfoService;
        private readonly Lazy<SettingsController> settingsController;
        private readonly FeedsController feedsController;
        private readonly ShellViewModel shellViewModel;
        private FeedManager feedManager;
        private DateTime lastUpdate;

        public AppController(INetworkInfoService networkInfoService, FeedsController feedsController, Lazy<SettingsController> settingsController, 
            ShellViewModel shellViewModel)
        {
            this.networkInfoService = networkInfoService;
            this.feedsController = feedsController;
            this.settingsController = settingsController;
            this.shellViewModel = shellViewModel;
            shellViewModel.ShowFeedViewCommand = feedsController.ShowFeedViewCommand;
            shellViewModel.RemoveFeedCommand = feedsController.RemoveFeedCommand;
            shellViewModel.FooterMenu = new[]
            {
                new NavigationItem("Settings", "\uf493") { Command = new AsyncDelegateCommand(() => 
                    shellViewModel.Navigate(this.settingsController.Value.SettingsViewModel)) }
            };
            shellViewModel.Initialize();
            MainView = shellViewModel.View;
        }

        public object MainView { get; }

        public async void Start()
        {
            // TODO:
            await Task.Delay(100);
            feedManager = new FeedManager();
            feedsController.FeedManager = feedManager;
            feedsController.Run();
            if (networkInfoService.InternetAccess) { lastUpdate = DateTime.Now; }
            shellViewModel.Feeds = feedManager.Feeds;
            settingsController.Value.FeedManager = feedManager;
        }

        public void Sleep()
        {
        }

        public void Resume()
        {
            if (networkInfoService.InternetAccess && DateTime.Now - lastUpdate > TimeSpan.FromMinutes(1))
            {
                feedsController.Update();
                lastUpdate = DateTime.Now;
            }
        }
    }
}
