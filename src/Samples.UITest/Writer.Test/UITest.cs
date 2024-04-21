using FlaUI.Core;
using FlaUI.Core.AutomationElements;
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
    private readonly string executable;
    private readonly List<string> usedFiles = [];
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
        Automation = new()
        {
            ConnectionTimeout = TimeSpan.FromSeconds(5)
        };
    }

    public ITestOutputHelper Log { get; }

    public UIA3Automation Automation { get; }

    public bool SkipAppClose { get; set; } = false;

    public Application Launch(LaunchArguments? arguments = null)
    {
        var args = (arguments ?? new LaunchArguments()).ToArguments();
        Log.WriteLine($"LaunchArguments: {args}");
        return app = Application.Launch(executable, args);
    }

    public ShellWindow GetShellWindow() => app!.GetMainWindow(Automation).As<ShellWindow>();

    public string GetTempFileName(string fileExtension)
    {
        var file = $"UITest_{Path.GetRandomFileName()}.{fileExtension}";
        file = Path.Combine(Path.GetTempPath(), file);
        usedFiles.Add(file);
        Log.WriteLine($"TempFile:        {file}");
        return file;
    }

    public void Dispose()
    {
        if (!SkipAppClose) app?.Close();
        app?.Dispose();
        Automation.Dispose();
        foreach (var file in usedFiles)
        {
            if (File.Exists(file)) File.Delete(file);
        }
    }
}
