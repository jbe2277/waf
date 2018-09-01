using NLog;
using NLog.Config;
using NLog.Targets;
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
    public partial class App : Application
    {
        private static readonly Tuple<string, LogLevel>[] logSettings =
        {
            Tuple.Create("App", LogLevel.Info),
            Tuple.Create("InfoMan.Common.P", LogLevel.Warn),
            Tuple.Create("InfoMan.Common.A", LogLevel.Warn),
            Tuple.Create("InfoMan.Infra.P", LogLevel.Warn),
            Tuple.Create("InfoMan.Infra.A", LogLevel.Warn),
            Tuple.Create("InfoMan.Address.P", LogLevel.Warn),
            Tuple.Create("InfoMan.Address.A", LogLevel.Warn),
            Tuple.Create("InfoMan.Address.D", LogLevel.Warn),
            Tuple.Create("InfoMan.Email.P", LogLevel.Warn),
            Tuple.Create("InfoMan.Email.A", LogLevel.Warn),
            Tuple.Create("InfoMan.Email.D", LogLevel.Warn),
        };

        private AggregateCatalog catalog;
        private CompositionContainer container;
        private IEnumerable<IModuleController> moduleControllers;

        public App()
        {
            var profileRoot = Path.Combine(AppDataPath, "ProfileOptimization");
            Directory.CreateDirectory(profileRoot);
            ProfileOptimization.SetProfileRoot(profileRoot);
            ProfileOptimization.StartProfile("Startup.profile");

            var fileTarget = new FileTarget("fileTarget")
            {
                FileName = Path.Combine(AppDataPath, "Log", "App.log"),
                Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.ff} ${level} ${processid} ${logger} ${message}  ${exception}",
                ArchiveAboveSize = 1024 * 1024 * 5,  // 5 MB
                MaxArchiveFiles = 2,
            };
            var logConfig = new LoggingConfiguration();
            logConfig.DefaultCultureInfo = CultureInfo.InvariantCulture;
            logConfig.AddTarget(fileTarget);
            var maxLevel = LogLevel.AllLoggingLevels.Last();
            foreach (var logSetting in logSettings)
            {
                logConfig.AddRule(logSetting.Item2, maxLevel, fileTarget, logSetting.Item1);
            }
            LogManager.Configuration = logConfig;
        }

        private static string AppDataPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                ApplicationInfo.ProductName);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Log.App.Info("{0} {1} is starting; OS: {2}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion);

            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;

            catalog = new AggregateCatalog();
            // Add the WinApplicationFramework assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IMessageService).Assembly));
            
            // Load module assemblies as well. See App.config file.
            foreach (string moduleAssembly in Settings.Default.ModuleAssemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(moduleAssembly));
            }

            container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            var batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            // Initialize all presentation services
            var presentationServices = container.GetExportedValues<IPresentationService>();
            foreach (var presentationService in presentationServices) { presentationService.Initialize(); }
            
            // Initialize and run all module controllers
            moduleControllers = container.GetExportedValues<IModuleController>();
            foreach (var moduleController in moduleControllers) { moduleController.Initialize(); }
            foreach (var moduleController in moduleControllers) { moduleController.Run(); }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            foreach (var moduleController in moduleControllers.Reverse()) { moduleController.Shutdown(); }
            container.Dispose();
            catalog.Dispose();
            Log.App.Info("{0} closed", ApplicationInfo.ProductName);
            base.OnExit(e);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, false);
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception, e.IsTerminating);
        }

        private static void HandleException(Exception e, bool isTerminating)
        {
            if (e == null) { return; }

            Log.App.Error(e, "Unhandled exception");
            if (!isTerminating)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, "Unknown application error\n\n{0}", e),
                    ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
