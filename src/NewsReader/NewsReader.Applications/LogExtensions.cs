using Microsoft.AppCenter.Crashes;
using NLog;

namespace Waf.NewsReader.Applications;

public static class LogExtensions
{
    public static void TrackError(this Logger log, Exception ex, [Localizable(false)] string message)
    {
        log.Error(ex, message);
        Crashes.TrackError(ex);
    }

    public static void TrackError(this Logger log, Exception ex, [Localizable(false)] string format, params object?[] arguments)
    {
        log.Error(ex, format, arguments);
        Crashes.TrackError(ex);
    }
}
