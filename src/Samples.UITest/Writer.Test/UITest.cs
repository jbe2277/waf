using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using UITest.Writer.Views;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace UITest.Writer;

public class UITest(ITestOutputHelper log) : UITestBase(log, "Writer/Release/net8.0-windows/writer.exe", "Samples.UITest/Writer/")
{
    public Application Launch(LaunchArguments? arguments = null)
    {
        var args = (arguments ?? new LaunchArguments()).ToArguments();
        Log.WriteLine($"Launch:          {args}");
        return App = Application.Launch(Executable, args);
    }

    public ShellWindow GetShellWindow() => App!.GetMainWindow(Automation).As<ShellWindow>();
}

public record LaunchArguments(string? UICulture = "en-US", string? Culture = "en-US", bool? DefaultSettings = true, string? AdditionalArguments = null) : LaunchArgumentsBase
{
    public override string ToArguments()
    {
        string?[] args = [CreateArg(UICulture), CreateArg(Culture), CreateArg(DefaultSettings), CreateArg(AdditionalArguments)];
        return string.Join(" ", args.Where(x => !string.IsNullOrEmpty(x)));
    }
}