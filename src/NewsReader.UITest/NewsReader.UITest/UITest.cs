using UITest.NewsReader.Views;

namespace UITest.NewsReader;

public class UITest() : UITestBase(
        androidApkFile: "NewsReaderAndroid/src/NewsReader/NewsReader.MauiSystem/bin/Release/net9.0-android/publish/Waf.NewsReader-Signed.apk",
        windowsAppId: "Waf.NewsReader_a8txtqew917ny!App",
        Environment.GetEnvironmentVariable("UITestOutputPath") ?? "out/NewsReader.UITest/")
{
    public ShellWindow GetShellWindow() => new(Driver);
}
