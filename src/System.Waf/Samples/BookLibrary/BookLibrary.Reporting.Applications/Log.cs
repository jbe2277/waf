using NLog;

namespace Waf.BookLibrary.Reporting.Applications
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("BookLib.Rep.A");
    }
}
