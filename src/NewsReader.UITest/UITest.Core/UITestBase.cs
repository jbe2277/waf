using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;
using System.Reflection;
using Xunit;

namespace UITest;

public abstract class UITestBase : IDisposable
{
    private readonly string? deviceId;
    private readonly string appId;
    private readonly string? app;
    private readonly string androidAppActivity;
    private readonly string rootPath;
    private readonly string testOutPath;

    protected UITestBase(string? deviceId, string appId, string? androidApkFile, string androidAppActivity, string? iosApp, string windowsAppId, string testOutputPath)
    {
        var devicePlatform = DeviceManager.GetDevicePlatform(GetType());
        this.deviceId = deviceId;
        this.appId = appId;
        this.androidAppActivity = androidAppActivity;
        var assemblyPath = Assembly.GetAssembly(typeof(UITestBase))!.Location;
        rootPath = Path.GetFullPath(Path.Combine(assemblyPath, "../../../../../../../"));
        testOutPath = Path.GetFullPath(Path.IsPathFullyQualified(testOutputPath) ? testOutputPath : Path.Combine(rootPath, testOutputPath));
        Directory.CreateDirectory(testOutPath);
        app = devicePlatform switch
        {
            DevicePlatform.Android => androidApkFile,
            DevicePlatform.IOS => iosApp,
            DevicePlatform.Windows => windowsAppId,
            _ => throw new NotSupportedException()
        };
        Log.WriteLine(("DevicePlatform:", $"{devicePlatform}"));
        Log.WriteLine(("DeviceId:", $"{deviceId}"));
        Log.WriteLine(("AppId:", $"{appId}"));
        Log.WriteLine(("App:", $"{app}"));
        Log.WriteLine(("AndroidAppActivity:", $"{androidAppActivity}"));
        Log.WriteLine(("TestOutPath:", $"{testOutPath}"));

        if (devicePlatform == DevicePlatform.Android)
        {
            Driver = SetupAndroid(new Uri("http://localhost:4723"));
        }
        else if (devicePlatform == DevicePlatform.IOS)
        {
            Driver = SetupIOS(new Uri("http://localhost:4723"));
        }
        else if (devicePlatform == DevicePlatform.Windows)
        {
            Driver = SetupWindows(new Uri("http://localhost:4723"));
        }
        else throw new NotSupportedException();
        
        Log.WriteLine("");
    }

    public AppiumDriver Driver { get; }

    public bool IsAndroid => Driver.IsAndroid;

    public bool IsIOS => Driver.IsIOS;

    public bool IsWindows => Driver.IsWindows;

    private AndroidDriver SetupAndroid(Uri serverUri)
    {
        // See: https://github.com/appium/appium-uiautomator2-driver
        var driverOptions = new AppiumOptions()
        {
            AutomationName = AutomationName.AndroidUIAutomator2,
            PlatformName = MobilePlatform.Android,
        };
        if (!string.IsNullOrEmpty(app))
        {
            var apk = Path.GetFullPath(Path.IsPathFullyQualified(app) ? app : Path.Combine(rootPath, app));
            driverOptions.App = apk;
        }
        else
        {
            driverOptions.AddAdditionalAppiumOption("appPackage", appId);
            driverOptions.AddAdditionalAppiumOption("appActivity", androidAppActivity);
        }
        return new(serverUri, driverOptions, TimeSpan.FromMinutes(3));
    }

    private IOSDriver SetupIOS(Uri serverUri)
    {
        // See: https://github.com/appium/appium-xcuitest-driver and https://appium.github.io/appium-xcuitest-driver/latest/reference/capabilities/
        var driverOptions = new AppiumOptions()
        {
            AutomationName = AutomationName.iOSXcuiTest,
            PlatformName = MobilePlatform.IOS,
        };
        if (!string.IsNullOrEmpty(app))
        {
            var appPath = Path.GetFullPath(Path.IsPathFullyQualified(app) ? app : Path.Combine(rootPath, app));
            driverOptions.App = appPath;
        }
        if (!string.IsNullOrEmpty(deviceId)) driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.Udid, deviceId);
        driverOptions.AddAdditionalAppiumOption("bundleId", appId);

        // TODO: xcode settings are required for deployment of the WebDriverAgent: https://appium.github.io/appium-xcuitest-driver/latest/preparation/prov-profile-basic-auto/
        //driverOptions.AddAdditionalAppiumOption("xcodeOrgId", "");
        //driverOptions.AddAdditionalAppiumOption("xcodeSigningId", "");
        
        driverOptions.AddAdditionalAppiumOption("wdaLaunchTimeout", 300_000);          // Increased timeouts for CI, as WebDriverAgent startup can be slow
        return new(serverUri, driverOptions, TimeSpan.FromMinutes(10));                // Increased timeout for CI, as simulator and WebDriverAgent startup can be slow
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
