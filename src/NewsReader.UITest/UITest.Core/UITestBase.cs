using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System.Reflection;
using Xunit;
using OpenQA.Selenium.Appium.Android;

namespace UITest;

public abstract class UITestBase : IDisposable
{
    private readonly string appId;
    private readonly string app;
    private readonly string androidAppActivity;
    private readonly string rootPath;
    private readonly string testOutPath;

    protected UITestBase(string appId, string androidApkFile, string androidAppActivity, string windowsAppId, string testOutputPath)
    {
        var devicePlatform = DeviceManager.GetDevicePlatform(GetType());

        this.appId = appId;
        this.androidAppActivity = androidAppActivity;
        var assemblyPath = Assembly.GetAssembly(typeof(UITestBase))!.Location;
        rootPath = Path.GetFullPath(Path.Combine(assemblyPath, "../../../../../../../"));
        testOutPath = Path.GetFullPath(Path.IsPathFullyQualified(testOutputPath) ? testOutputPath : Path.Combine(rootPath, testOutputPath));
        Directory.CreateDirectory(testOutPath);
        app = devicePlatform switch
        {
            DevicePlatform.Android => androidApkFile,
            DevicePlatform.Windows => windowsAppId,
            _ => throw new NotSupportedException()
        };
        Log.WriteLine(("DevicePlatform:", $"{devicePlatform}"));
        Log.WriteLine(("AppId:", $"{appId}"));
        Log.WriteLine(("App:", $"{app}"));
        Log.WriteLine(("AndroidAppActivity:", $"{androidAppActivity}"));
        Log.WriteLine(("TestOutPath:", $"{testOutPath}"));

        if (devicePlatform == DevicePlatform.Android)
        {
            Driver = SetupAndroid(new Uri("http://localhost:4723"));
        }
        else if (devicePlatform == DevicePlatform.Windows)
        {
            Driver = SetupWindows(new Uri("http://localhost:4723"));
        }
        else throw new NotSupportedException();
        
        Log.WriteLine("");
    }

    public AppiumDriver Driver { get; }

    public bool IsAndroid => Driver.IsAndroid();

    public bool IsIOS => Driver.IsIOS();

    public bool IsWindows => Driver.IsWindows();

    private AndroidDriver SetupAndroid(Uri serverUri)
    {
        var apk = Path.GetFullPath(Path.IsPathFullyQualified(app) ? app : Path.Combine(rootPath, app));

        // See: https://github.com/appium/appium-uiautomator2-driver
        var driverOptions = new AppiumOptions()
        {
            AutomationName = AutomationName.AndroidUIAutomator2,
            PlatformName = MobilePlatform.Android,
            App = apk,
        };
        // TODO: Use this instead of App for local dev
        //driverOptions.AddAdditionalAppiumOption("appPackage", appId);
        //driverOptions.AddAdditionalAppiumOption("appActivity", androidAppActivity);
        return new(serverUri, driverOptions, TimeSpan.FromMinutes(3));
    }

    private WindowsDriver SetupWindows(Uri serverUri)
    {
        // See: https://github.com/appium/appium-windows-driver
        var driverOptions = new AppiumOptions()
        {
            AutomationName = "Windows",
            PlatformName = MobilePlatform.Windows,
            App = app
        };
        return new WindowsDriver(serverUri, driverOptions, TimeSpan.FromMinutes(3));
    }

    public void CreateScreenshot(string name)
    {
        var fileName = GetScreenshotFile(name);
        Log.WriteLine("Screenshot: " + fileName);
        Driver.GetScreenshot().SaveAsFile(fileName);
    }

    private string GetScreenshotFile(string fileName)
    {
        var file = Path.Combine(testOutPath, string.Join("-", TestContext.Current.TestClass!.TestClassSimpleName, TestContext.Current.TestMethod!.MethodName, fileName));
        if (string.IsNullOrEmpty(Path.GetExtension(file))) file += ".png";
        return file;
    }

    public virtual void Dispose()
    {
        if (TestContext.Current.TestState?.Result == TestResult.Failed) TryGetScreenshot();
        Driver.Dispose();

        void TryGetScreenshot()
        {
            try
            {
                CreateScreenshot("Fail");
            }
            catch (Exception ex)
            {
                Log.WriteLine($"ERROR: Dispose: Failed to create a screenshot: {ex.Message}");
            }
        }
    }
}
