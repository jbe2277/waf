using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Diagnostics;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Applications.Controllers;
using Xamarin.Forms;

namespace Waf.NewsReader.Presentation
{
    // Add App.xaml.key.cs file with:
    //public partial class App
    //{
    //    static partial void GetAppCenterSecret(ref string appSecret)
    //    {
    //        appSecret = "android={key};uwp={key};ios={key}";
    //    }
    //}

    public partial class App : Application
    {
        private readonly IAppController appController;

        public App(Lazy<IAppController> appController)
        {
            InitializeComponent();
            this.appController = appController.Value;
            MainPage = (Page)this.appController.MainView;
        }

        public static void InitializeLogging(TraceSource system)
        {
            system.Switch.Level = SourceLevels.All;
            Log.Default.Switch.Level = SourceLevels.All;
        }

        protected override void OnStart()
        {
            Log.Default.Info("App started");
            string appSecret = null;
            GetAppCenterSecret(ref appSecret);
            if (appSecret != null) AppCenter.Start(appSecret, typeof(Analytics), typeof(Crashes));

            appController.Start();
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

        static partial void GetAppCenterSecret(ref string appSecret);
    }
}
