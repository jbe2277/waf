using Microsoft.AppCenter.Crashes;
using NLog;

namespace Waf.NewsReader;  // Use here the root namespace - as this should be available in all upper layers.

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
