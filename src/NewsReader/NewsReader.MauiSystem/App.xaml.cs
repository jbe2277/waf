using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using System.Diagnostics;
using System.Globalization;
using System.Waf.Applications.Services;
using Waf.NewsReader.Applications.Controllers;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Presentation.Services;
using Waf.NewsReader.Applications;
using System.Waf.Foundation;

namespace Waf.NewsReader.MauiSystem;

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
    private readonly IAppInfoService appInfoService;
    private readonly IAppController appController;

    public App(ISettingsService settingsService, IAppInfoService appInfoService, Lazy<IAppController> appController, ILocalizationService? localizationService = null, 
        SystemTraceListener? systemTraceListener = null)
    {
        this.settingsService = settingsService;
        this.appInfoService = appInfoService;
        InitializeLogging(Log.Default, systemTraceListener);
        localizationService?.Initialize();
        InitializeCultures(settingsService.Get<AppSettings>());

        InitializeComponent();
        this.appController = appController.Value;
        MainPage = (Page)this.appController.MainView;
    }

    protected override void OnStart()
    {
        Log.Default.Info("App started ({0}, {1}) on {2}", appInfoService.AppName, appInfoService.VersionString, DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ssK", CultureInfo.InvariantCulture));
        string? appSecret = null;
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

    private static void InitializeLogging(TraceSource system, TraceListener? systemListener = null)
    {
        system.Switch.Level = SourceLevels.All;
        Log.Default.Switch.Level = SourceLevels.All;

        var sources = new[]
        {
            system,
            Log.Default
        };
        foreach (var source in sources)
        {
            source.Listeners.Clear();
            source.Listeners.Add(systemListener ?? new AppTraceListener(showTime: false));
        }
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

    static partial void GetAppCenterSecret(ref string? appSecret);
}
