using System;
using System.Diagnostics;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Applications.Controllers;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Presentation.Views;
using Xamarin.Forms;

namespace Waf.NewsReader.Presentation
{
    public partial class App : Application
    {
        private readonly IAppController appController;
        private readonly Lazy<ShellViewModel> shellViewModel;

        public App(Lazy<IAppController> appController, Lazy<ShellViewModel> shellViewModel)
        {
            InitializeComponent();
            this.appController = appController.Value;
            this.shellViewModel = shellViewModel;
        }

        public static void InitializeLogging(TraceSource system)
        {
            system.Switch.Level = SourceLevels.All;
            Log.Default.Switch.Level = SourceLevels.All;
        }

        protected override void OnStart()
        {
            Log.Default.Info("App started");
            appController.Start();

            var shellView = (ShellView)shellViewModel.Value.View;
            shellViewModel.Value.Initialize();
            MainPage = shellView;
        }

        protected override void OnSleep()
        {
            Log.Default.Info("App sleep");
            appController.Sleep();
        }

        protected override void OnResume()
        {
            Log.Default.Info("App resume");
            appController.Resume();
        }
    }
}
