using UITest.NewsReader.Views;

namespace UITest.NewsReader;

public class UITest() : UITestBase("Waf.NewsReader",        
        androidApkFile: Environment.GetEnvironmentVariable("UITestApkFile") ?? "src/NewsReader/NewsReader.MauiSystem/bin/Release/net9.0-android/Waf.NewsReader-Signed.apk",
        androidAppActivity: "Waf.NewsReader.MainActivity",
        windowsAppId: "Waf.NewsReader_a8txtqew917ny!App",
        Environment.GetEnvironmentVariable("UITestOutputPath") ?? "out/NewsReader.UITest/")
{
    public ShellWindow GetShellWindow() => new(Driver);
}
