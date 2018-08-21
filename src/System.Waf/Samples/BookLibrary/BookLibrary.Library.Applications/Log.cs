using NLog;

namespace Waf.BookLibrary.Library.Applications
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("BookLib.Lib.A");
    }
}
