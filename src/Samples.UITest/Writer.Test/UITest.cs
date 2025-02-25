﻿using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using System.Diagnostics;
using UITest.Writer.Views;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace UITest.Writer;

public abstract class UITest() : UITestBase("Writer.exe",
        Environment.GetEnvironmentVariable("UITestExePath") ?? "out/Writer/Release/net8.0-windows/",
        Environment.GetEnvironmentVariable("UITestOutputPath") ?? "out/Samples.UITest/Writer/")
{
    public Application Launch(LaunchArguments? arguments = null, bool resetSettings = true)
    {
        Log.WriteLine("");
        if (resetSettings)
        {
            var productName = FileVersionInfo.GetVersionInfo(Executable).ProductName ?? throw new InvalidOperationException("Could not read the ProductName from the exe.");
            var settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), productName, "Settings", "Settings.xml");
            if (File.Exists(settingsFile)) File.Delete(settingsFile);
            Log.WriteLine($"Delete settings: {settingsFile}");
        }
        var args = (arguments ?? new LaunchArguments()).ToArguments();
        Log.WriteLine($"Launch:          {args}");
        return App = Application.Launch(Executable, args);
    }

    public ShellWindow GetShellWindow() => (App!.GetMainWindow(Automation) ?? throw new ElementFoundException("MainWindow not found")).As<ShellWindow>();
}

public record LaunchArguments(string? UICulture = "en-US", string? Culture = "en-US", string? AdditionalArguments = null) : LaunchArgumentsBase
{
    public override string ToArguments()
    {
        string?[] args = [CreateArg(UICulture), CreateArg(Culture), AdditionalArguments];
        return string.Join(" ", args.Where(x => !string.IsNullOrEmpty(x)));
    }
}