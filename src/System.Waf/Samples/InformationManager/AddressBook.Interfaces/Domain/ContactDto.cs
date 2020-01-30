namespace Waf.InformationManager.AddressBook.Interfaces.Domain
{
    /// <summary>
    /// Contains contact information. This is a data transfer object that can be used to pass information over module boundaries.
    /// </summary>
    public sealed class ContactDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDto"/> class.
        /// </summary>
        /// <param name="firstname">The first name.</param>
        /// <param name="lastname">The last name.</param>
        /// <param name="email">The email.</param>
        public ContactDto(string? firstname, string? lastname, string? email)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
        }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string? Firstname { get; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string? Lastname { get; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string? Email { get; }
    }
}
