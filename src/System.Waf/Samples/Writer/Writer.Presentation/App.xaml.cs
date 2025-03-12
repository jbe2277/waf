using NLog.Targets.Wrappers;
using NLog.Targets;
using NLog;
using System.Globalization;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows;
using System.Windows.Markup;
using Waf.Writer.Applications.Properties;
using Microsoft.Extensions.Configuration;
using Waf.Writer.Presentation.Properties;
using Autofac;
using Waf.Writer.Applications;
using IContainer = Autofac.IContainer;

namespace Waf.Writer.Presentation;

public partial class App
{
    private static readonly Lazy<string> logFileName = new(() => ((FileTarget)((AsyncTargetWrapper)LogManager.Configuration.FindTargetByName("fileTarget")).WrappedTarget).FileName.Render(new LogEventInfo()));
    private IContainer? container;
    private IReadOnlyList<IModuleController> moduleControllers = [];

    public static string LogFileName => logFileName.Value;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Log.App.Info("{0} {1} is starting; OS: {2}; .NET: {3}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion, Environment.Version);

#if (!DEBUG)
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += (_, ea) => Log.Default.Warn(ea.Exception, "UnobservedTaskException");
#endif
        AppConfig appConfig;
        try
        {
            var config = new ConfigurationBuilder().AddCommandLine(Environment.GetCommandLineArgs()).Build();
            appConfig = config.Get<AppConfig>() ?? new AppConfig();
        }
        catch (Exception ex)
        {
            Log.Default.Error(ex, "Command line parsing error");
            appConfig = new AppConfig();
        }

        var builder = new ContainerBuilder();
        builder.RegisterModule(new ApplicationsModule());
        builder.RegisterModule(new PresentationModule());
        container = builder.Build();

        var settingsService = container.Resolve<ISettingsService>();
        settingsService.ErrorOccurred += (_, e) => Log.Default.Error(e.Error, "Error in SettingsService");
        var appSettings = settingsService.Get<AppSettings>();
        InitializeCultures(appConfig, appSettings);
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        moduleControllers = container.Resolve<IReadOnlyList<IModuleController>>();
        foreach (var x in moduleControllers) x.Initialize();
        foreach (var x in moduleControllers) x.Run();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        foreach (var x in moduleControllers.Reverse()) x.Shutdown();
        container?.Dispose();
        Log.App.Info("{0} closed", ApplicationInfo.ProductName);
        base.OnExit(e);
    }

    private static void InitializeCultures(AppConfig appConfig, AppSettings settings)
    {
        try
        {
            if (!string.IsNullOrEmpty(appConfig.Culture)) Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(appConfig.Culture);

            var uiCulture = !string.IsNullOrEmpty(appConfig.UICulture) ? appConfig.UICulture : settings.UICulture;
            if (!string.IsNullOrEmpty(uiCulture)) Thread.CurrentThread.CurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(uiCulture);
        }
        catch (Exception ex)
        {
            Log.Default.Error(ex, "The specified culture code is invalid");
        }
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception ?? throw new InvalidOperationException("Unknown exception object");
        Log.Default.Error(ex, "UnhandledException; IsTerminating={0}", e.IsTerminating);

        var message = string.Format(CultureInfo.CurrentCulture, Presentation.Properties.Resources.UnknownError, ex);
        if (MainWindow?.IsVisible == true)
        {
            MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
        }
    }
}
