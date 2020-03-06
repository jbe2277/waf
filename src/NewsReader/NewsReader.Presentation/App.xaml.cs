using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Waf.Applications.Services;
using System.Waf.Foundation;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Applications.Controllers;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
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
        private readonly ISettingsService settingsService;
        private readonly IAppController appController;

        public App(ISettingsService settingsService, Lazy<IAppController> appController, ILocalizationService localizationService = null)
        {
            this.settingsService = settingsService;
            localizationService?.Initialize();
            InitializeCultures(settingsService.Get<AppSettings>());

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
            settingsService.Save();
        }

        protected override void OnResume()
        {
            Log.Default.Info("App resume");
            appController.Resume();
        }

        private static void InitializeCultures(AppSettings appSettings)
        {
            if (!string.IsNullOrEmpty(appSettings.Language))
            {
                var culture = new CultureInfo(appSettings.Language);
                CultureInfo.CurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture = culture;
                CultureInfo.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture = culture;
            }
        }

        static partial void GetAppCenterSecret(ref string appSecret);
    }
}
