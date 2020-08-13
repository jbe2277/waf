using Autofac;
using Foundation;
using Microsoft.Identity.Client;
using UIKit;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Presentation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Waf.NewsReader.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("SwipeView_Experimental");
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

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
        }
    }
}
