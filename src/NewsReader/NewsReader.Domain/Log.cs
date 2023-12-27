using NLog;

namespace Waf.NewsReader.Domain;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("Dom");
}
