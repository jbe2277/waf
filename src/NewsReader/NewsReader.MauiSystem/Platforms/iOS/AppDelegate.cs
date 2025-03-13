using Foundation;

namespace Waf.NewsReader.MauiSystem.Platforms.iOS;

[Register("AppDelegate")]
internal sealed class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
