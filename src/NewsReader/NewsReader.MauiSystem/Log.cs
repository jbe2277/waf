using NLog;

namespace Waf.NewsReader.MauiSystem;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("Sys");
}

