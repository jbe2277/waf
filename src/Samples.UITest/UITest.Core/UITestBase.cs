using FlaUI.Core;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace UITest;

public abstract class UITestBase : IDisposable
{
    private readonly List<string> usedFiles = [];

    static UITestBase()
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

        Mouse.MovePixelsPerMillisecond = 2;
        Retry.DefaultTimeout = TimeSpan.FromSeconds(5);
        Retry.DefaultInterval = TimeSpan.FromMilliseconds(250);
    }

    protected UITestBase(ITestOutputHelper log, string relativeExecutablePath, string relativeTestOutputPath)
    {
        Log = log;
        var assemblyPath = Assembly.GetAssembly(typeof(UITestBase))!.Location;
        var outPath = Path.GetFullPath(Path.Combine(assemblyPath, "../../../../../../../out/"));
        Executable = Path.GetFullPath(Path.Combine(outPath, relativeExecutablePath));
        TestOutPath = Path.GetFullPath(Path.Combine(outPath, relativeTestOutputPath));
        Directory.CreateDirectory(TestOutPath);
        Log.WriteLine($"OSVersion:       {Environment.OSVersion}");
        Log.WriteLine($"ProcessorCount:  {Environment.ProcessorCount}");
        Log.WriteLine($"MachineName:     {Environment.MachineName}");
        Log.WriteLine($"UserInteractive: {Environment.UserInteractive}");
        Log.WriteLine($"Executable:      {Executable}");
        Log.WriteLine($"TestOutPath:     {TestOutPath}");
        Automation = new()
        {
            ConnectionTimeout = TimeSpan.FromSeconds(5)
        };
    }

    public ITestOutputHelper Log { get; }

    public string Executable {  get; }

    public string TestOutPath { get; }

    public UIA3Automation Automation { get; }

    public Application? App { get; protected set; }

    public bool SkipAppClose { get; set; } = false;

    public string GetTempFileName(string fileExtension)
    {
        var file = $"UITest_{Path.GetRandomFileName()}.{fileExtension}";
        file = Path.Combine(Path.GetTempPath(), file);
        usedFiles.Add(file);
        Log.WriteLine($"TempFile:        {file}");
        return file;
    }

    public string GetScreenshotFile(string fileName, [CallerMemberName] string? memberName = null)
            => Path.Combine(TestOutPath, string.Join("-", new[] { memberName, fileName }.Where(x => !string.IsNullOrEmpty(x))));

    public void Dispose()
    {
        if (!SkipAppClose) App?.Close();
        App?.Dispose();
        Automation.Dispose();
        foreach (var file in usedFiles)
        {
            if (File.Exists(file)) File.Delete(file);
        }
    }
}
