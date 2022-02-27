using NLog;

namespace Waf.InformationManager.EmailClient.Modules.Domain;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Email.D");
}
