using NLog;

namespace Waf.BookLibrary.Library.Presentation;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("BookLib.Lib.P");

    public static Logger App { get; } = LogManager.GetLogger("App");
}
