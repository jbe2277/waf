using FlaUI.Core;
using FlaUI.Core.Capturing;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

namespace UITest;

public abstract class UITestBase : IDisposable
{
    private readonly List<string> usedFiles = [];
    private string? testMethodName;

    static UITestBase()
    {
        NativeMethods.SetProcessDPIAware();
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

        Mouse.MovePixelsPerMillisecond = 2;
        Retry.DefaultTimeout = TimeSpan.FromSeconds(5);
        Retry.DefaultInterval = TimeSpan.FromMilliseconds(250);
    }

    protected UITestBase(ITestOutputHelper log, string executableFileName, string executablePath, string testOutputPath)
    {
        Log = log;
        var assemblyPath = Assembly.GetAssembly(typeof(UITestBase))!.Location;
        var rootPath = Path.GetFullPath(Path.Combine(assemblyPath, "../../../../../../../"));
        Executable = Path.GetFullPath(Path.Combine(Path.IsPathFullyQualified(executablePath) ? executablePath : Path.Combine(rootPath, executablePath), executableFileName));
        TestOutPath = Path.GetFullPath(Path.IsPathFullyQualified(testOutputPath) ? testOutputPath : Path.Combine(rootPath, testOutputPath));
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

    public string Executable { get; }

    public string TestOutPath { get; }

    public string TestMethodName => testMethodName ?? throw new InvalidOperationException("Test context not available. Use the Run method for your test code.");

    public UIA3Automation Automation { get; }

    public Application? App { get; protected set; }

    public bool SkipAppClose { get; set; } = false;

    public void Run(Action action, [CallerMemberName] string? memberName = null)
    {
        try
        {
            testMethodName = memberName;
            action();
        }
        catch (Exception)
        {
            TryGetScreenshot();
            throw;
        }
        finally
        {
            testMethodName = null;
        }

        void TryGetScreenshot()
        {
            try
            {
                Capture.Screen().ToFile(GetScreenshotFile("Fail"));
            }
            catch { }
        }
    }

    public string GetTempFileName(string fileExtension)
    {
        var file = $"UITest_{Path.GetRandomFileName()}.{fileExtension}";
        file = Path.Combine(Path.GetTempPath(), file);
        usedFiles.Add(file);
        Log.WriteLine($"TempFile:        {file}");
        return file;
    }

    public string GetScreenshotFile(string fileName)
    {
        var file = Path.Combine(TestOutPath, string.Join("-", TestMethodName, fileName));
        if (string.IsNullOrEmpty(Path.GetExtension(file))) file += ".png";
        return file;
    }

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


    private static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDPIAware();
    }
}
