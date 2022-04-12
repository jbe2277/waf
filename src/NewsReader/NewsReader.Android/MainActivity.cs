using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Presentation;
using Autofac;
using Android.Content;
using Microsoft.Identity.Client;
using Waf.NewsReader.Android.Services;

namespace Waf.NewsReader.Android
{
    [Activity(Label = "NewsReader", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private App? application;

        public static MainActivity? Current { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Current = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Forms.SetFlags("SwipeView_Experimental");

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);

            App.InitializeLogging(Log.Default, new AndroidTraceListener());

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApplicationsModule());
            builder.RegisterModule(new PresentationModule());
            builder.RegisterModule(new AndroidModule());
            var container = builder.Build();
            application = container.Resolve<App>();
            LoadApplication(application);
        }

        protected override void OnPause()
        {
            base.OnPause();
            application?.OnPause();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }
    }
}