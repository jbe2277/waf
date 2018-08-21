using NLog;

namespace Waf.InformationManager.EmailClient.Modules.Presentation
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Email.P");
    }
}
