using NLog;

namespace Waf.InformationManager.Assembler;

internal static class Log
{
    public static Logger App { get; } = LogManager.GetLogger("App");
}
