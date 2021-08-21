using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows;
using System.Windows.Threading;
using Waf.InformationManager.Assembler.Properties;
using Waf.InformationManager.Common.Applications.Services;

namespace Waf.InformationManager.Assembler
{
    public partial class App
    {
        private static readonly (string loggerNamePattern, LogLevel minLevel)[] logSettings =
        {
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
        };

        private AggregateCatalog? catalog;
        private CompositionContainer? container;
        private IEnumerable<IModuleController>? moduleControllers;

        public App()
        {
            var profileRoot = Path.Combine(AppDataPath, "ProfileOptimization");
            Directory.CreateDirectory(profileRoot);
            ProfileOptimization.SetProfileRoot(profileRoot);
            ProfileOptimization.StartProfile("Startup.profile");

            var layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.ff} [${level:format=FirstCharacter}] ${processid} ${logger} ${message}  ${exception:format=tostring}";
            var fileTarget = new AsyncTargetWrapper("fileTarget", new FileTarget()
                {
                    FileName = Path.Combine(AppDataPath, "Log", "InfoMan.log"),
                    Layout = layout,
                    ArchiveAboveSize = 5_000_000,  // 5 MB
                    MaxArchiveFiles = 1,
                    ArchiveNumbering = ArchiveNumberingMode.Rolling
                })
            { OverflowAction = AsyncTargetWrapperOverflowAction.Block };
            var traceTarget = new AsyncTargetWrapper("traceTarget", new TraceTarget()
                {
                    Layout = layout,
                    RawWrite = true
                })
            { OverflowAction = AsyncTargetWrapperOverflowAction.Block };

            var logConfig = new LoggingConfiguration();
            logConfig.DefaultCultureInfo = CultureInfo.InvariantCulture;
            logConfig.AddTarget(fileTarget);
            logConfig.AddTarget(traceTarget);
            foreach (var (loggerNamePattern, minLevel) in logSettings)
            {
                var rule = new LoggingRule(loggerNamePattern, minLevel, fileTarget);
                rule.Targets.Add(traceTarget);
                logConfig.LoggingRules.Add(rule);
            }
            LogManager.Configuration = logConfig;
        }

        private static string AppDataPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Log.App.Info("{0} {1} is starting; OS: {2}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion);

#if (!DEBUG)
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif
            catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IMessageService).Assembly));   // WinApplicationFramework

            // Load module assemblies as well. See App.config file.
            foreach (var x in Settings.Default.ModuleAssemblies) catalog.Catalogs.Add(new AssemblyCatalog(x));

            container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            var batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            // Initialize all presentation services
            var presentationServices = container.GetExportedValues<IPresentationService>();
            foreach (var x in presentationServices) x.Initialize();
            
            // Initialize and run all module controllers
            moduleControllers = container.GetExportedValues<IModuleController>();
            foreach (var x in moduleControllers) x.Initialize();
            foreach (var x in moduleControllers) x.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (moduleControllers is not null) foreach (var x in moduleControllers.Reverse()) x.Shutdown();
            container?.Dispose();
            catalog?.Dispose();
            Log.App.Info("{0} closed", ApplicationInfo.ProductName);
            base.OnExit(e);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) => HandleException(e.Exception, false);

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e) => HandleException(e.ExceptionObject as Exception, e.IsTerminating);

        private static void HandleException(Exception? e, bool isTerminating)
        {
            if (e == null) return;
            Log.App.Error(e, "Unhandled exception");
            if (!isTerminating)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, "Unknown application error\n\n{0}", e), ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
