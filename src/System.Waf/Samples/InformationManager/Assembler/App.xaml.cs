using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows;
using Waf.InformationManager.Assembler.Properties;
using Waf.InformationManager.Common.Applications.Services;
using Waf.InformationManager.Common.Presentation;
using IContainer = Autofac.IContainer;

namespace Waf.InformationManager.Assembler;

public partial class App
{
    private static readonly (string loggerNamePattern, LogLevel minLevel)[] logSettings =
    [
        ("App", LogLevel.Info),
        ("InfoMan.Common.P", LogLevel.Warn),
        ("InfoMan.Common.A", LogLevel.Warn),
        ("InfoMan.Infra.P", LogLevel.Warn),
        ("InfoMan.Infra.A", LogLevel.Warn),
        ("InfoMan.Address.P", LogLevel.Warn),
        ("InfoMan.Address.A", LogLevel.Warn),
        ("InfoMan.Address.D", LogLevel.Warn),
        ("InfoMan.Email.P", LogLevel.Warn),
        ("InfoMan.Email.A", LogLevel.Warn),
        ("InfoMan.Email.D", LogLevel.Warn),
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
                FileName = AppInfo.LogFileName,
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

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Log.App.Info("{0} {1} is starting; OS: {2}; .NET: {3}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion, Environment.Version);

#if (!DEBUG)
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += (_, ea) => Log.App.Warn(ea.Exception, "UnobservedTaskException");
#endif
        AppConfig appConfig;
        try
        {
            var config = new ConfigurationBuilder().AddCommandLine(Environment.GetCommandLineArgs()).Build();
            appConfig = config.Get<AppConfig>() ?? new AppConfig();
        }
        catch (Exception ex)
        {
            Log.App.Error(ex, "Command line parsing error");
            appConfig = new AppConfig();
        }

        var builder = new ContainerBuilder();
        builder.RegisterModule(new CommonPresentationModule());

        // Load module assemblies as well. See App.config file.
        var baseDir = AppContext.BaseDirectory;
        AssemblyLoadContext.Default.Resolving += ResolvingExtensions;
        foreach (var x in Settings.Default.ModuleAssemblies)
        {
            var module = Type.GetType(x ?? "") ?? throw new InvalidOperationException("Type not found: " + x);
            builder.RegisterModule((IModule)Activator.CreateInstance(module)!);
        }

        container = builder.Build();

        InitializeCultures(appConfig);
        var presentationServices = container.Resolve<IReadOnlyList<IPresentationService>>();
        foreach (var x in presentationServices) x.Initialize();

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
            Log.App.Error(ex, "The specified culture code is invalid");
        }
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception ?? throw new InvalidOperationException("Unknown exception object");
        Log.App.Error(ex, "UnhandledException; IsTerminating={0}", e.IsTerminating);

        var message = string.Format(CultureInfo.CurrentCulture, "Unknown application error\n\n{0}", ex);
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
