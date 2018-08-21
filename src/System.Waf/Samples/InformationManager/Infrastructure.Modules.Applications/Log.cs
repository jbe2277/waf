using NLog;

namespace Waf.InformationManager.Infrastructure.Modules.Applications
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Infra.A");
    }
}
