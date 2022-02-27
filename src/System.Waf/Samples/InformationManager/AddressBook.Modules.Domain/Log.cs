using NLog;

namespace Waf.InformationManager.AddressBook.Modules.Domain;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Address.D");
}
