using NLog;

namespace Waf.NewsReader.Presentation;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("Pre");
}

