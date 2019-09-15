using System;
using System.Threading.Tasks;
using System.Waf.Applications;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers
{
    internal class AppController : IAppController
    {
        private readonly Lazy<SettingsController> settingsController;
        private readonly ShellViewModel shellViewModel;
        private readonly Lazy<FeedViewModel> feedViewModel;
        private readonly AsyncDelegateCommand showFeedViewCommand;
        private FeedManager feedManager;

        public AppController(Lazy<SettingsController> settingsController, ShellViewModel shellViewModel, Lazy<FeedViewModel> feedViewModel)
        {
            this.settingsController = settingsController;
            this.shellViewModel = shellViewModel;
            this.feedViewModel = new Lazy<FeedViewModel>(() => InitializeViewModel(feedViewModel.Value));
            showFeedViewCommand = new AsyncDelegateCommand(ShowFeedView);
            shellViewModel.ShowFeedViewCommand = showFeedViewCommand;
            shellViewModel.FooterMenu = new[]
            {
                new NavigationItem("Settings", "\uf493") { Command = new AsyncDelegateCommand(() => shellViewModel.PushAsync(this.settingsController.Value.SettingsViewModel)) }
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
            shellViewModel.Feeds = feedManager.Feeds;
            settingsController.Value.FeedManager = feedManager;
        }

        public void Sleep()
        {
        }

        public void Resume()
        {
        }

        private Task ShowFeedView(object parameter)
        {
            feedViewModel.Value.Feed = (Feed)parameter;
            return shellViewModel.PushAsync(feedViewModel.Value);
        }

        private FeedViewModel InitializeViewModel(FeedViewModel viewModel)
        {
            // TODO:
            //viewModel.RefreshCommand = newsFeedsController.Value.RefreshFeedCommand;
            //viewModel.ReadUnreadCommand = newsFeedsController.Value.ReadUnreadCommand;
            //viewModel.ShowFeedItemViewCommand = showFeedItemViewCommand;
            return viewModel;
        }
    }
}
