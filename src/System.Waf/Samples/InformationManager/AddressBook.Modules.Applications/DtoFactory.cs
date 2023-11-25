using Waf.InformationManager.AddressBook.Interfaces.Domain;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Applications;

internal static class DtoFactory
{
    [return: NotNullIfNotNull(nameof(contact))]
    public static ContactDto? ToDto(this Contact? contact) => contact != null ? new(contact.Firstname, contact.Lastname, contact.Email) : null;
}
