using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using UITest.BookLibrary.Views;
using Xunit.Abstractions;

namespace UITest.BookLibrary;

public class UITest(ITestOutputHelper log) : UITestBase(log, "BookLibrary/Release/net8.0-windows/BookLibrary.exe", "Samples.UITest/BookLibrary/")
{
    public Application Launch(LaunchArguments? arguments = null)
    {
        var args = (arguments ?? new LaunchArguments()).ToArguments();
        Log.WriteLine($"Launch:          {args}");
        return App = Application.Launch(Executable, args);
    }

    public ShellWindow GetShellWindow() => App!.GetMainWindow(Automation).As<ShellWindow>();
}

public record LaunchArguments(string? AdditionalArguments = null) : LaunchArgumentsBase
{
    public override string ToArguments()
    {
        string?[] args = [CreateArg(AdditionalArguments)];
        return string.Join(" ", args.Where(x => !string.IsNullOrEmpty(x)));
    }
}