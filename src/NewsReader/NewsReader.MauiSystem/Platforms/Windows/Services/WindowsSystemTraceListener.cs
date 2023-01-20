using System.Diagnostics;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.MauiSystem.Platforms.Windows.Services;

internal sealed class WindowsSystemTraceListener : SystemTraceListener
{
    public override TraceListener Create() => new WindowsTraceListener();
}

internal sealed class WindowsTraceListener : AppTraceListener
{
    public WindowsTraceListener() : base(Log) { }

    private static void Log(TraceEventType eventType, string message) => Trace.WriteLine(message);
}
