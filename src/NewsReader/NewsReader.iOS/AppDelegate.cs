using Foundation;
using UIKit;

namespace Waf.NewsReader.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new Presentation.App());
            return base.FinishedLaunching(app, options);
        }
    }
}
