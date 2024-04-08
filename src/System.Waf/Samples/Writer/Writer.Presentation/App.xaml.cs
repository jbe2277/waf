using NLog.Targets.Wrappers;
using NLog.Targets;
using NLog;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Reflection;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.ViewModels;
using Microsoft.Extensions.Configuration;
using Waf.Writer.Presentation.Properties;

namespace Waf.Writer.Presentation;

public partial class App
{
    private static readonly Lazy<string> logFileName = new(() => ((FileTarget)((AsyncTargetWrapper)LogManager.Configuration.FindTargetByName("fileTarget")).WrappedTarget).FileName.Render(new LogEventInfo()));
    private AggregateCatalog? catalog;
    private CompositionContainer? container;
    private IEnumerable<IModuleController> moduleControllers = Array.Empty<IModuleController>();

    public static string LogFileName => logFileName.Value;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Log.App.Info("{0} {1} is starting; OS: {2}; .NET: {3}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion, Environment.Version);

#if (!DEBUG)
        DispatcherUnhandledException += AppDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
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

        catalog = new();
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(IMessageService).Assembly));  // WinApplicationFramework
        catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));   // Writer.Presentation
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(ShellViewModel).Assembly));   // Writer.Applications
        container = new(catalog, CompositionOptions.DisableSilentRejection);
        var batch = new CompositionBatch();
        batch.AddExportedValue(container);
        container.Compose(batch);

        var settingsService = container.GetExportedValue<ISettingsService>();
        settingsService.ErrorOccurred += (_, e) => Log.Default.Error(e.Error, "Error in SettingsService");
        InitializeCultures(appConfig, settingsService.Get<AppSettings>());
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        moduleControllers = container.GetExportedValues<IModuleController>();
        foreach (var x in moduleControllers) x.Initialize();
        foreach (var x in moduleControllers) x.Run();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        foreach (var x in moduleControllers.Reverse()) x.Shutdown();
        container?.Dispose();
        catalog?.Dispose();
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

    private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) => HandleException(e.Exception, false);

    private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e) => HandleException(e.ExceptionObject as Exception, e.IsTerminating);

    private static void HandleException(Exception? e, bool isTerminating)
    {
        if (e == null) return;
        Log.App.Error(e, "Unhandled exception");
        if (!isTerminating)
        {
            MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Presentation.Properties.Resources.UnknownError, e), ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
