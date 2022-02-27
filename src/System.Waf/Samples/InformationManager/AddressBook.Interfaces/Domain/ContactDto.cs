namespace Waf.InformationManager.AddressBook.Interfaces.Domain;

/// <summary>Contains contact information. This is a data transfer object that can be used to pass information over module boundaries.</summary>
/// <param name="Firstname">The first name.</param>
/// <param name="Lastname">The last name.</param>
/// <param name="Email">The email.</param>
public record ContactDto(string? Firstname, string? Lastname, string? Email);
