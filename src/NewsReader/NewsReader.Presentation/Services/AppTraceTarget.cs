using NLog;
using NLog.Targets;

namespace Waf.NewsReader.Presentation.Services;

internal sealed class AppTraceTarget : TargetWithLayout
{
    protected override void Write(LogEventInfo logEvent)
    {
        string message = RenderLogEvent(Layout, logEvent);
#if ANDROID
        var x = logEvent.Level;
        if (x == LogLevel.Fatal || x == LogLevel.Error)
            Android.Util.Log.Error(logEvent.LoggerName, message);
        else if (x == LogLevel.Warn)
            Android.Util.Log.Warn(logEvent.LoggerName, message);
        else if (x == LogLevel.Info)
            Android.Util.Log.Info(logEvent.LoggerName, message);
        else
            Android.Util.Log.Verbose(logEvent.LoggerName, message);
#elif IOS
        Console.WriteLine(message);
#else
        System.Diagnostics.Trace.WriteLine(message);
#endif
    }
}