using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Waf.InformationManager.AddressBook.Interfaces.Domain
{
    /// <summary>
    /// Contains contact informations. This is a data transfer object that can be used to pass information over module boundaries.
    /// </summary>
    public sealed class ContactDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDto"/> class.
        /// </summary>
        /// <param name="firstname">The first name.</param>
        /// <param name="lastname">The last name.</param>
        /// <param name="email">The email.</param>
        public ContactDto(string firstname, string lastname, string email)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Email = email;
        }


        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string Firstname { get; private set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string Lastname { get; private set; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; private set; }
    }
}
