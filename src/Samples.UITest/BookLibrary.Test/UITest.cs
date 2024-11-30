using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using System.Diagnostics;
using UITest.BookLibrary.Views;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace UITest.BookLibrary;

public record AppInfo(string CompanyName, string ProductName, string SettingsFile, string DatabaseFile);

public abstract class UITest : UITestBase
{
    protected UITest(ITestOutputHelper log) : base(log, "BookLibrary.exe",
        Environment.GetEnvironmentVariable("UITestExePath") ?? "out/BookLibrary/Release/net8.0-windows/",
        Environment.GetEnvironmentVariable("UITestOutputPath") ?? "out/Samples.UITest/BookLibrary/")
    {
        var companyName = FileVersionInfo.GetVersionInfo(Executable).CompanyName ?? throw new InvalidOperationException("Could not read the CompanyName from the exe.");
        var productName = FileVersionInfo.GetVersionInfo(Executable).ProductName ?? throw new InvalidOperationException("Could not read the ProductName from the exe.");
        AppInfo = new(companyName, productName,
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), productName, "Settings", "Settings.xml"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), companyName, productName, "Resources", "BookLibrary.db"));
    }

    protected AppInfo AppInfo { get; }

    public Application Launch(LaunchArguments? arguments = null, bool resetSettings = true, bool resetDatabase = true)
    {
        Log.WriteLine("");
        if (resetSettings)
        {
            if (File.Exists(AppInfo.SettingsFile)) File.Delete(AppInfo.SettingsFile);
            Log.WriteLine($"Delete settings: {AppInfo.SettingsFile}");
        }
        if (resetDatabase)
        {
            if (File.Exists(AppInfo.DatabaseFile)) File.Delete(AppInfo.DatabaseFile);
            Log.WriteLine($"Delete database: {AppInfo.DatabaseFile}");
        }
        var args = (arguments ?? new LaunchArguments()).ToArguments();
        Log.WriteLine($"Launch:          {args}");
        return App = Application.Launch(Executable, args);
    }

    public ShellWindow GetShellWindow() => App!.GetMainWindow(Automation).As<ShellWindow>();
}

public record LaunchArguments(string? UICulture = "en-US", string? Culture = "en-US", string? AdditionalArguments = null) : LaunchArgumentsBase
{
    public override string ToArguments()
    {
        string?[] args = [CreateArg(UICulture), CreateArg(Culture), AdditionalArguments];
        return string.Join(" ", args.Where(x => !string.IsNullOrEmpty(x)));
    }
}