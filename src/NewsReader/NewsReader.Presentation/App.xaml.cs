using Jbe.NewsReader.Applications.Controllers;
using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.ExternalServices;
using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.Globalization;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.Xaml;

namespace Jbe.NewsReader.Presentation
{
    sealed partial class App : Application
    {
        private IEnumerable<IAppController> appControllers;
        private bool isInitialized;


        public App()
        {
            var themeSettings = ApplicationData.Current.LocalSettings.Values[SettingsKey.Theme];
            if (themeSettings != null)
            {
                RequestedTheme = (ApplicationTheme)themeSettings;
            }
            var languageSettings = ApplicationData.Current.LocalSettings.Values[SettingsKey.Language] as string;
            if (string.IsNullOrEmpty(languageSettings))
            {
                ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];
            }
            else
            {
                ApplicationLanguages.PrimaryLanguageOverride = languageSettings;
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(languageSettings);
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(languageSettings);
            }

            InitializeComponent();
            Suspending += OnSuspending;
            Resuming += OnResuming;
        }
        

        private void Initialize()
        {
            if (isInitialized) { return; }
            isInitialized = true;
            
            var configuration = new ContainerConfiguration()
                .WithAssembly(typeof(ShellViewModel).GetTypeInfo().Assembly)
                .WithAssembly(typeof(AppInfoService).GetTypeInfo().Assembly)
                .WithAssembly(typeof(App).GetTypeInfo().Assembly);
            var container = configuration.CreateContainer();

            // Initialize and run all module controllers
            // TODO: Dirty hack because of a serious bug in MEF2 (Multiple Exports toghether with Shared)
            appControllers = container.GetExports<INavigationService>().Cast<IAppController>().ToArray();
            foreach (var appController in appControllers) { appController.Initialize(); }
            foreach (var appController in appControllers) { appController.Run(); }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Initialize();
        }

        private void OnResuming(object sender, object e)
        {
            foreach (var appController in appControllers) { appController.Resuming(); }
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            using (var session = new ExtendedExecutionSession())
            {
                session.Reason = ExtendedExecutionReason.SavingData;
                var requestTask = session.RequestExtensionAsync().AsTask();
                await Task.WhenAll(appControllers.Select(x => x.SuspendingAsync()).Concat(new[] { requestTask }).ToArray()).ConfigureAwait(false);
            }
            deferral.Complete();
        }
    }
}
