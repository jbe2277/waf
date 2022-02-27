using NLog;

namespace Waf.InformationManager.AddressBook.Modules.Applications;

internal static class Log
{
    public static Logger Default { get; } = LogManager.GetLogger("InfoMan.Address.A");
}
