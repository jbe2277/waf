using NLog;

namespace Waf.Writer.Presentation
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("Writer.P");

        public static Logger App { get; } = LogManager.GetLogger("App");
    }
}
