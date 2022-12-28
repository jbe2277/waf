using System.Diagnostics;
using System.Globalization;

namespace Waf.NewsReader.Presentation.Services;

public sealed class AppTraceListener : TraceListener
{
    private readonly Action<string> log;
    private readonly bool showTime;

    public AppTraceListener(Action<string>? messageAction = null, bool showTime = true)
    {
        log = messageAction ?? Console.WriteLine;
        this.showTime = showTime;
    }

    public override void Write(string? message) { }

    public override void WriteLine(string? message) { }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id)
    {
        log(TimeUtcNow() + Format(eventType));
    }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
    {
        log(TimeUtcNow() + Format(eventType) + " " + message);
    }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? format, params object?[]? args)
    {
        log(TimeUtcNow() + Format(eventType) + " " + string.Format(CultureInfo.InvariantCulture, format ?? "", args ?? Array.Empty<object>()));
    }

    private string TimeUtcNow()
    {
        return showTime ? DateTime.UtcNow.ToString("HH:mm:ss.ff", CultureInfo.InvariantCulture) + " " : "";
    }

    private static string Format(TraceEventType eventType)
    {
        return eventType switch
        {
            TraceEventType.Critical => "[E]",
            TraceEventType.Error => "[E]",
            TraceEventType.Warning => "[W]",
            TraceEventType.Information => "[I]",
            TraceEventType.Verbose => "[D]",
            _ => "[_]"
        };
    }
}
