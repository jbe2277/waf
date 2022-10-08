using Foundation;
using UIKit;

namespace Waf.NewsReader.Presentation.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    // TODO:
    //public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
    //{
    //    Microsoft.Identity.Client.AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
    //    return true;
    //}
}
