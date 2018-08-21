using NLog;

namespace Waf.InformationManager.Common.Presentation
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Common.P");
    }
}
