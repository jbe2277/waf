using NLog;

namespace Waf.BookLibrary.Library.Domain;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("BookLib.Lib.D");
}
