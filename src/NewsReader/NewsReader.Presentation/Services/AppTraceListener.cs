using System.Diagnostics;
using System.Globalization;

namespace Waf.NewsReader.Presentation.Services;

public class AppTraceListener : TraceListener
{
    private readonly Action<TraceEventType, string> log;
    private readonly bool showTime;

    public AppTraceListener(Action<TraceEventType, string>? messageAction = null, bool showTime = true)
    {
        log = messageAction ?? LogToConsole;
        this.showTime = showTime;
    }

    public override void Write(string? message) { }

    public override void WriteLine(string? message) { }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id)
    {
        log(eventType, TimeUtcNow() + Format(eventType));
    }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
    {
        log(eventType, TimeUtcNow() + Format(eventType) + " " + message);
    }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? format, params object?[]? args)
    {
        log(eventType, TimeUtcNow() + Format(eventType) + " " + string.Format(CultureInfo.InvariantCulture, format ?? "", args ?? Array.Empty<object>()));
    }

    private string TimeUtcNow()
    {
        return showTime ? DateTime.UtcNow.ToString("HH:mm:ss.ff", CultureInfo.InvariantCulture) + " " : "";
    }

    private static void LogToConsole(TraceEventType logLevel, string message) => Console.WriteLine(message);

    private static string Format(TraceEventType eventType) => eventType switch
    {
        TraceEventType.Critical => "[E]",
        TraceEventType.Error => "[E]",
        TraceEventType.Warning => "[W]",
        TraceEventType.Information => "[I]",
        TraceEventType.Verbose => "[D]",
        _ => "[_]"
    };
}
