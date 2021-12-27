using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Presentation
{
    public partial class App
    {
        private AggregateCatalog? catalog;
        private CompositionContainer? container;
        private IEnumerable<IModuleController> moduleControllers = Array.Empty<IModuleController>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Log.App.Info("{0} {1} is starting; OS: {2}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion);

#if (!DEBUG)
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif
            catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IMessageService).Assembly));  // WinApplicationFramework
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));   // Writer.Presentation
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ShellViewModel).Assembly));   // Writer.Applications
            container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            var batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            var settingsService = container.GetExportedValue<ISettingsService>();
            settingsService.ErrorOccurred += (_, e) => Log.Default.Error(e.Error, "Error in SettingsService");
            InitializeCultures(settingsService.Get<AppSettings>());
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

        private static void InitializeCultures(AppSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.Culture)) CultureInfo.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(settings.Culture);
            if (!string.IsNullOrEmpty(settings.UICulture)) CultureInfo.CurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(settings.UICulture);
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
}
