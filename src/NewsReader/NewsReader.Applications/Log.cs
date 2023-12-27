using NLog;

namespace Waf.NewsReader.Applications;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("App");
}
