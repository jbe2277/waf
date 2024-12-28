﻿using System.Globalization;
using System.Waf.Applications.Services;
using Waf.NewsReader.Applications.Controllers;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using LogLevel = NLog.LogLevel;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.Presentation;

public partial class App : Application
{
    private static readonly (string loggerNamePattern, LogLevel minLevel)[] logSettings =
    [
        ("Dom", LogLevel.Trace),
        ("App", LogLevel.Trace),
        ("Pre", LogLevel.Trace),
        ("Sys", LogLevel.Trace),
    ];

    private readonly ISettingsService settingsService;
    private readonly IAppInfoService appInfoService;
    private readonly IAppController appController;
    private bool isCreated;

    public App(ISettingsService settingsService, IAppInfoService appInfoService, Lazy<IAppController> appController, ILocalizationService? localizationService = null)
    {
        this.settingsService = settingsService;
        this.appInfoService = appInfoService;
        InitializeLogging();

        settingsService.ErrorOccurred += (_, e) => Log.Default.Error("SettingsService error: {0}" + e.Error);
        settingsService.FileName = Path.Combine(FileSystem.AppDataDirectory, "Settings.xml");
        localizationService?.Initialize();
        InitializeCultures(settingsService.Get<AppSettings>());

        InitializeComponent();
        this.appController = appController.Value;
    }

    public static string LogFileName { get; } = Path.Combine(FileSystem.CacheDirectory, "Logging", "AppLog.txt");

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window((Page)appController.MainView)
        {
            Title = AppInfo.Name,
            MinimumWidth = 300,
            MinimumHeight = 400
        };
        window.Created += (_, _) => OnCreated();
        window.Deactivated += (_, _) => OnDeactivated();
        window.Stopped += (_, _) => OnStopped();
        window.Destroying += (_, _) => OnDestroying();
        window.Resumed += (_, _) => OnResumed();
        return window;
    }

    private void OnCreated()
    {
        if (isCreated) return;   // On Android it seems that this might be called more than once
        isCreated = true;

        Log.Default.Info("App started {0}, {1} on {2}", appInfoService.AppName, appInfoService.VersionString, DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ssK", CultureInfo.InvariantCulture));
        Log.Default.Info("Device: {0} {1} {2}; Platform: {3} {4}", DeviceInfo.Idiom, DeviceInfo.Manufacturer, DeviceInfo.Model, DeviceInfo.Platform, DeviceInfo.Version);
        appController.Start();
    }

    private void OnDeactivated()
    {
        Log.Default.Info("App deactivated");
#if !ANDROID   // Android: don't use onPause to save data. See: https://developer.android.com/guide/components/activities/activity-lifecycle
        Save();
#endif
    }

    private void OnStopped()
    {
        Log.Default.Info("App stopped");
        Save();
    }

    private void OnDestroying()
    {
        Log.Default.Info("App destroying");
        Save();
    }

    private void Save()
    {
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
        LogManager.Setup().LoadConfiguration(c =>
        {
            c.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;
            var layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.ff} [${level:format=FirstCharacter}] ${logger} ${message} ${exception}";
            var fileTarget = c.ForTarget("fileTarget").WriteTo(new FileTarget
            {
                FileName = LogFileName,
                Layout = layout,
                ArchiveAboveSize = 5_000_000,  // 5 MB
                MaxArchiveFiles = 1,
                ArchiveNumbering = ArchiveNumberingMode.Rolling
            }).WithAsync(AsyncTargetWrapperOverflowAction.Block);
#if DEBUG
            var traceTarget = c.ForTarget("traceTarget").WriteTo(new AppTraceTarget
            {
                Layout = layout,
            }).WithAsync(AsyncTargetWrapperOverflowAction.Block);
#endif

            foreach (var (loggerNamePattern, minLevel) in logSettings)
            {
                c.ForLogger(loggerNamePattern).FilterMinLevel(minLevel).WriteTo(fileTarget)
#if DEBUG
                    .WriteTo(traceTarget)
#endif
                ;
            }
        });
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
}
