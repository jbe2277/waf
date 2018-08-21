using NLog;

namespace Waf.Writer.Applications
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("Writer.A");
    }
}
