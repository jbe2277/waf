using NLog;

namespace Waf.BookLibrary.Reporting.Presentation
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("BookLib.Rep.P");
    }
}
