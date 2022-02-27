using NLog;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Infra.P");
}
