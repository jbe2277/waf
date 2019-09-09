using Autofac;
using Foundation;
using UIKit;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Presentation;
using Xamarin.Forms;

namespace Waf.NewsReader.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();

            App.InitializeLogging(Log.Default);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApplicationsModule());
            builder.RegisterModule(new PresentationModule());
            builder.RegisterModule(new IosModule());
            var container = builder.Build();
            LoadApplication(container.Resolve<App>());

            return base.FinishedLaunching(app, options);
        }
    }
}
