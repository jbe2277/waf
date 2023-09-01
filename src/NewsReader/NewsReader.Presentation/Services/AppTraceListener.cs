using System.Diagnostics;
using System.Globalization;

namespace Waf.NewsReader.Presentation.Services;

internal sealed class AppTraceListener : TraceListener
{
    public override void Write(string? message) { }

    public override void WriteLine(string? message) { }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id)
    {
        Log(eventType, source, TimeNow() + Format(eventType));
    }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
    {
        Log(eventType, source, TimeNow() + Format(eventType) + " " + message);
    }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? format, params object?[]? args)
    {
        Log(eventType, source, TimeNow() + Format(eventType) + " " + string.Format(CultureInfo.InvariantCulture, format ?? "", args ?? Array.Empty<object>()));
    }

    private static string TimeNow()
    {
#if IOS
        return "";
#else
        return DateTime.UtcNow.ToString("HH:mm:ss.ff", CultureInfo.InvariantCulture) + " ";
#endif
    }

    private static string Format(TraceEventType eventType) => eventType switch
    {
        TraceEventType.Critical => "[E]",
        TraceEventType.Error => "[E]",
        TraceEventType.Warning => "[W]",
        TraceEventType.Information => "[I]",
        TraceEventType.Verbose => "[D]",
        _ => "[_]"
    };

    private static void Log(TraceEventType logLevel, string source, string message)
    {
#if ANDROID
        switch (logLevel)
        {
            case TraceEventType.Critical:
            case TraceEventType.Error:
                global::Android.Util.Log.Error(source, message);
                break;
            case TraceEventType.Warning:
                global::Android.Util.Log.Warn(source, message);
                break;
            case TraceEventType.Information:
                global::Android.Util.Log.Info(source, message);
                break;
            default:
                global::Android.Util.Log.Verbose(source, message);
                break;
        }
#elif IOS
        Console.WriteLine(source + " " + message);
#else
        Trace.WriteLine(source + " " + message);
#endif
    }
}