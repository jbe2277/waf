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

namespace Waf.NewsReader.Presentation;

// Telemetry data collection with https://appcenter.ms  
// Provide the secrets via separate file 'App.xaml.keys.cs' which will be excluded from GIT:
//public partial class App
//{
//    static partial void GetAppCenterSecret(ref string? appSecret)
//    {
//        appSecret = "android={secret};windowsdesktop={secret};ios={secret}";
//    }
//}

public partial class App : Application
{
    private readonly ISettingsService settingsService;
    private readonly IAppInfoService appInfoService;
    private readonly IAppController appController;

    public App(ISettingsService settingsService, IAppInfoService appInfoService, Lazy<IAppController> appController, ILocalizationService? localizationService = null)
    {
        this.settingsService = settingsService;
        this.appInfoService = appInfoService;
        InitializeLogging();

        settingsService.ErrorOccurred += (_, e) => Log.Default.Error("SettingsService error: {0}" + e.Error);
        localizationService?.Initialize();
        InitializeCultures(settingsService.Get<AppSettings>());

        InitializeComponent();
        this.appController = appController.Value;
        MainPage = (Page)this.appController.MainView;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Title = AppInfo.Name;
        window.MinimumWidth = 300;
        window.MinimumHeight = 400;
        window.Created += (_, _) => OnCreated();
        window.Deactivated += (_, _) => OnDeactivated();
#if WINDOWS
        window.Destroying += (_, _) => OnDeactivated();
#endif
        window.Resumed += (_, _) => OnResumed();
        return window;
    }

    private void OnCreated()
    {
        Log.Default.Info("App started {0}, {1} on {2}", appInfoService.AppName, appInfoService.VersionString, DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ssK", CultureInfo.InvariantCulture));
        Log.Default.Info("Device: {0} {1} {2}; Platform: {3} {4}", DeviceInfo.Idiom, DeviceInfo.Manufacturer, DeviceInfo.Model, DeviceInfo.Platform, DeviceInfo.Version);
        string? appSecret = null;
        GetAppCenterSecret(ref appSecret);
        if (appSecret != null) AppCenter.Start(appSecret, typeof(Analytics), typeof(Crashes));
        appController.Start();
    }

    private void OnDeactivated()
    {
        Log.Default.Info("App deactivated");
        appController.Save();
        settingsService.Save();
    }

    private void OnResumed()
    {
        Log.Default.Info("App resumed");
        appController.Update();
    }

    private static void InitializeLogging()
    {
        Log.Default.Switch.Level = SourceLevels.All;
        var sources = new[]
        {
            Log.Default
        };
        foreach (var source in sources)
        {
            source.Listeners.Clear();
            source.Listeners.Add(new AppTraceListener());
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
