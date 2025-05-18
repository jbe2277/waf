using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System.Reflection;
using Xunit;

namespace UITest.NewsReader;

public abstract class UITestBase : IDisposable
{
    protected UITestBase(string appId, string testOutputPath)
    {
        var assemblyPath = Assembly.GetAssembly(typeof(UITestBase))!.Location;
        var rootPath = Path.GetFullPath(Path.Combine(assemblyPath, "../../../../../../../"));
        AppId = appId;
        TestOutPath = Path.GetFullPath(Path.IsPathFullyQualified(testOutputPath) ? testOutputPath : Path.Combine(rootPath, testOutputPath));
        Directory.CreateDirectory(TestOutPath);
        Log.WriteLine($"OSVersion:       {Environment.OSVersion}");
        Log.WriteLine($"ProcessorCount:  {Environment.ProcessorCount}");
        Log.WriteLine($"MachineName:     {Environment.MachineName}");
        Log.WriteLine($"UserInteractive: {Environment.UserInteractive}");
        Log.WriteLine($"AppId:           {AppId}");
        Log.WriteLine($"TestOutPath:     {TestOutPath}");

        Driver = SetupWindows(new Uri("http://localhost:4723"));
        Log.WriteLine("");
    }

    public ITestOutputHelper Log { get; } = TestContext.Current.TestOutputHelper ?? throw new InvalidOperationException("Test context not available.");

    public string AppId { get; }

    public string TestOutPath { get; }

    public AppiumDriver Driver { get; }

    private WindowsDriver SetupWindows(Uri serverUri)
    {
        // See: https://github.com/appium/appium-windows-driver
        var driverOptions = new AppiumOptions()
        {
            AutomationName = "Windows",
            PlatformName = MobilePlatform.Windows,
            App = AppId
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
        var file = Path.Combine(TestOutPath, string.Join("-", TestContext.Current.TestClass!.TestClassSimpleName, TestContext.Current.TestMethod!.MethodName, fileName));
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
