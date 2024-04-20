using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using System.Reflection;
using UITest.Writer.Views;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace UITest.Writer;

public class UITest : IDisposable
{
    private const string arguments = "--UICulture=en-US --Culture=en-US";
    private readonly string executable;
    private Application? app;

    static UITest()
    {
        Mouse.MovePixelsPerMillisecond = 2;
        Retry.DefaultTimeout = TimeSpan.FromSeconds(5);
        Retry.DefaultInterval = TimeSpan.FromMilliseconds(250);
    }

    public UITest(ITestOutputHelper log)
    {
        Log = log;
        var assemblyPath = Assembly.GetAssembly(typeof(UITest))!.Location;
        executable = Path.GetFullPath(Path.Combine(assemblyPath, "../../../../../../../out/Writer/Release/net8.0-windows/writer.exe"));

        Log.WriteLine($"OSVersion:       {Environment.OSVersion}");
        Log.WriteLine($"ProcessorCount:  {Environment.ProcessorCount}");
        Log.WriteLine($"MachineName:     {Environment.MachineName}");
        Log.WriteLine($"UserInteractive: {Environment.UserInteractive}");
        Log.WriteLine($"AssemblyPath:    {assemblyPath}");
        Log.WriteLine($"Executable:      {executable}");
        Log.WriteLine($"Arguments:       {arguments}");
        Log.WriteLine("");
        Automation = new()
        {
            ConnectionTimeout = TimeSpan.FromSeconds(5)
        };
    }

    public ITestOutputHelper Log { get; }

    public UIA3Automation Automation { get; }

    public Application Launch() => app = Application.Launch(executable, arguments);

    public ShellWindow GetShellWindow(bool maximize = false) 
    { 
        var x = app!.GetMainWindow(Automation);
        if (maximize) x.Patterns.Window.Pattern.SetWindowVisualState(WindowVisualState.Maximized);
        return x.As<ShellWindow>();
    }

    public void Dispose()
    {
        app?.Close();
        app?.Dispose();
        Automation.Dispose();
    }
}
