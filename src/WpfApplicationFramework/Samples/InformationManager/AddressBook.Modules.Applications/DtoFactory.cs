using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.InformationManager.AddressBook.Interfaces.Domain;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Applications
{
    internal static class DtoFactory
    {
        public static ContactDto ToDto(this Contact contact)
        {
            return contact != null ? new ContactDto(contact.Firstname, contact.Lastname, contact.Email) : null;
        }
    }
}
