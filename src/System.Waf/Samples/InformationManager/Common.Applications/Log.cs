using NLog;

namespace Waf.InformationManager.Common.Applications
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Common.A");
    }
}
