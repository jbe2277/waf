using NLog;

namespace Waf.InformationManager.EmailClient.Modules.Applications;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Email.A");
}
