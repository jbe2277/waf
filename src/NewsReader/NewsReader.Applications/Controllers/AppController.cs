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
        private FeedManager feedManager;

        public AppController(Lazy<SettingsController> settingsController, ShellViewModel shellViewModel)
        {
            this.settingsController = settingsController;
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
            settingsController.Value.FeedManager = feedManager;
        }

        public void Sleep()
        {
        }

        public void Resume()
        {
        }
    }
}
