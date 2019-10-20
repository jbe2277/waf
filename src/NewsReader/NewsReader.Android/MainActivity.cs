using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Presentation;
using Autofac;

namespace Waf.NewsReader.Android
{
    [Activity(Label = "NewsReader", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Current { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Current = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);

            App.InitializeLogging(Log.Default);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApplicationsModule());
            builder.RegisterModule(new PresentationModule());
            builder.RegisterModule(new AndroidModule());
            var container = builder.Build();
            LoadApplication(container.Resolve<App>());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}