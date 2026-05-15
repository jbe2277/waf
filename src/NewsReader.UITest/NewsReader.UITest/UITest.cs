using UITest.NewsReader.Views;

namespace UITest.NewsReader;

public class UITest() : UITestBase(
        deviceId: Environment.GetEnvironmentVariable("UITestDeviceId"),
        appId: "Waf.NewsReader",        
        androidApkFile: Environment.GetEnvironmentVariable("UITestApkFile") ?? "src/NewsReader/NewsReader.MauiSystem/bin/Release/net10.0-android/Waf.NewsReader-Signed.apk",
        androidAppActivity: "Waf.NewsReader.MainActivity",
        iosApp: Environment.GetEnvironmentVariable("UITestApp"),
        windowsAppId: "Waf.NewsReader_a8txtqew917ny!App",
        testOutputPath: Environment.GetEnvironmentVariable("UITestOutputPath") ?? "out/NewsReader.UITest/")
{
    public ShellWindow GetShellWindow() => new(Driver);
}
