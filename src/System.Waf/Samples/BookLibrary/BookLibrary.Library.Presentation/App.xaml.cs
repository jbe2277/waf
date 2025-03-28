using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Waf.BookLibrary.Library.Applications;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Presentation.Properties;
using IContainer = Autofac.IContainer;

namespace Waf.BookLibrary.Library.Presentation;

public partial class App
{
    private static readonly (string loggerNamePattern, LogLevel minLevel)[] logSettings =
    [
        ("App", LogLevel.Info),
        ("BookLib.Lib.P", LogLevel.Info),
        ("BookLib.Lib.A", LogLevel.Info),
        ("BookLib.Lib.D", LogLevel.Info),
        ("BookLib.Rep.P", LogLevel.Info),
        ("BookLib.Rep.A", LogLevel.Info),
    ];

    private IContainer? container;
    private IReadOnlyList<IModuleController> moduleControllers = [];

    public App()
    {
        LogManager.Setup().LoadConfiguration(c =>
        {
            c.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;
            var layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.ff} [${level:format=FirstCharacter}] ${processid} ${logger} ${message} ${exception}";
            var fileTarget = c.ForTarget("fileTarget").WriteTo(new FileTarget
            {
                FileName = LogFileName,
                Layout = layout,
                ConcurrentWrites = true,
                ArchiveAboveSize = 5_000_000,  // 5 MB
                MaxArchiveFiles = 1,
                ArchiveNumbering = ArchiveNumberingMode.Rolling
            }).WithAsync(AsyncTargetWrapperOverflowAction.Block);
            var traceTarget = c.ForTarget("traceTarget").WriteTo(new TraceTarget
            {
                Layout = layout,
                RawWrite = true
            }).WithAsync(AsyncTargetWrapperOverflowAction.Block);

            foreach (var (loggerNamePattern, minLevel) in logSettings)
            {
                c.ForLogger(loggerNamePattern).FilterMinLevel(minLevel).WriteTo(fileTarget).WriteTo(traceTarget);
            }
        });
    }

    private static string AppDataPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName);

    public static string LogFileName { get; } = Path.Combine(AppDataPath, "Log", "BookLibrary.log");

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

        // Load module assemblies as well (e.g. Reporting extension). See App.config file.
        var baseDir = AppContext.BaseDirectory;
        AssemblyLoadContext.Default.Resolving += ResolvingExtensions;
        foreach (var x in Settings.Default.ModuleAssemblies)
        {
            var module = Type.GetType(x ?? "") ?? throw new InvalidOperationException("Type not found: " + x);
            builder.RegisterModule((IModule)Activator.CreateInstance(module)!);
        }

        container = builder.Build();

        InitializeCultures(appConfig);
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        moduleControllers = container.Resolve<IReadOnlyList<IModuleController>>();
        foreach (var x in moduleControllers) x.Initialize();
        foreach (var x in moduleControllers) x.Run();


        static Assembly? ResolvingExtensions(AssemblyLoadContext context, AssemblyName name)
        {
            var path = Path.Combine(AppContext.BaseDirectory, name.Name + ".dll");
            if (!File.Exists(path)) return null;
            return context.LoadFromAssemblyPath(path);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        foreach (var x in moduleControllers.Reverse()) x.Shutdown();
        container?.Dispose();
        Log.App.Info("{0} closed", ApplicationInfo.ProductName);
        base.OnExit(e);
    }

    private static void InitializeCultures(AppConfig appConfig)
    {
        try
        {
            if (!string.IsNullOrEmpty(appConfig.Culture)) Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(appConfig.Culture);
            if (!string.IsNullOrEmpty(appConfig.UICulture)) Thread.CurrentThread.CurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(appConfig.UICulture);
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
