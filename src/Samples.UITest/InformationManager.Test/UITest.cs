using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using System.Diagnostics;
using UITest.InformationManager.Views;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace UITest.InformationManager;

public abstract class UITest(ITestOutputHelper log) : UITestBase(log, "InformationManager.exe",
        Environment.GetEnvironmentVariable("UITestExePath") ?? "out/InformationManager/Release/net8.0-windows/",
        Environment.GetEnvironmentVariable("UITestOutputPath") ?? "out/Samples.UITest/InformationManager/")
{
    public Application Launch(LaunchArguments? arguments = null, bool resetSettings = true, bool resetContainer = true)
    {
        Log.WriteLine("");
        if (resetSettings)
        {
            var productName = FileVersionInfo.GetVersionInfo(Executable).ProductName ?? throw new InvalidOperationException("Could not read the ProductName from the exe.");
            var settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), productName, "Settings", "Settings.xml");
            if (File.Exists(settingsFile)) File.Delete(settingsFile);
            Log.WriteLine($"Delete settings: {settingsFile}");
        }
        if (resetContainer)
        {
            var companyName = FileVersionInfo.GetVersionInfo(Executable).CompanyName ?? throw new InvalidOperationException("Could not read the CompanyName from the exe.");
            var productName = FileVersionInfo.GetVersionInfo(Executable).ProductName ?? throw new InvalidOperationException("Could not read the ProductName from the exe.");
            var containerFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), companyName, productName, "InformationManager.datx");
            if (File.Exists(containerFile)) File.Delete(containerFile);
            Log.WriteLine($"Delete file:     {containerFile}");
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