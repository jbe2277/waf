using NLog;

namespace Waf.InformationManager.AddressBook.Modules.Presentation
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Address.P");
    }
}
