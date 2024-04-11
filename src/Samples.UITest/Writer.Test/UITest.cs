using FlaUI.Core;
using FlaUI.UIA3;
using System.Reflection;
using UITest.Writer.Views;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace UITest.Writer;

public class UITest : IDisposable
{
    private const string arguments = "--UICulture=en-US";
    private readonly string executable;

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
    }

    public ITestOutputHelper Log { get; }

    public UIA3Automation Automation { get; } = new();

    public Application Launch() => Application.Launch(executable, arguments); 

    public ShellWindow GetShellWindow(Application app) => new(app.GetMainWindow(Automation));

    public void Dispose() => Automation.Dispose();
}
