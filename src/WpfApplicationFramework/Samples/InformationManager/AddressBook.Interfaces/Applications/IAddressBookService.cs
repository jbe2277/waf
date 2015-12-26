using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.InformationManager.AddressBook.Interfaces.Domain;

namespace Waf.InformationManager.AddressBook.Interfaces.Applications
{
    /// <summary>
    /// Exposes the address book to other modules.
    /// </summary>
    public interface IAddressBookService
    {
        /// <summary>
        /// Shows a view which allows the user to select a contact.
        /// </summary>
        /// <param name="ownerView">The owner view.</param>
        /// <returns>The selected contact or null when the user canceled this operation.</returns>
        ContactDto ShowSelectContactView(object ownerView);
    }
}
