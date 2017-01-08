using Jbe.NewsReader.Applications.Controllers;
using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.ExternalServices;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
            var themeSettings = ApplicationData.Current.LocalSettings.Values["Theme"];
            if (themeSettings != null)
            {
                RequestedTheme = (ApplicationTheme)themeSettings;
            }
            InitializeComponent();
            Suspending += OnSuspending;
            Resuming += OnResuming;
        }


        private void Initialize()
        {
            if (isInitialized) { return; }
            isInitialized = true;

            ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];
            //ApplicationLanguages.PrimaryLanguageOverride = "en-US";

            var configuration = new ContainerConfiguration()
                .WithAssembly(typeof(ShellViewModel).GetTypeInfo().Assembly)
                .WithAssembly(typeof(AppInfoService).GetTypeInfo().Assembly)
                .WithAssembly(typeof(App).GetTypeInfo().Assembly);
            var container = configuration.CreateContainer();

            // Initialize and run all module controllers
            appControllers = container.GetExports<IAppController>();
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
            await Task.WhenAll(appControllers.Select(x => x.SuspendingAsync())).ConfigureAwait(false);
            deferral.Complete();
        }
    }
}
