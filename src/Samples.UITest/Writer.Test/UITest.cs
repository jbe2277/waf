using FlaUI.Core;
using FlaUI.UIA3;
using System.Reflection;
using UITest.Writer.Views;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace UITest.Writer;

public class UITest(ITestOutputHelper log) : IDisposable
{
    public ITestOutputHelper Log => log;

    public UIA3Automation Automation { get; } = new();

    public Application Launch() 
    {
        var assemblyPath = Assembly.GetAssembly(typeof(UITest))!.Location;
        Log.WriteLine($"AssemblyPath: {assemblyPath}");
        var writerPath = Path.GetFullPath(Path.Combine(assemblyPath, "../../../../../../../out/Writer/Release/net8.0-windows/writer.exe"));
        Log.WriteLine($"WriterPath: {writerPath}");
        return Application.Launch(writerPath, "--UICulture=en-US"); 
    }

    public ShellWindow GetShellWindow(Application app) => new(app.GetMainWindow(Automation));

    public void Dispose() => Automation.Dispose();
}
