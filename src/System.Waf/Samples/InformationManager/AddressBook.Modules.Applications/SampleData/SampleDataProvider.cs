using System.CodeDom.Compiler;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Applications.SampleData;

[GeneratedCode("ToSuppressCodeAnalysis", "1.0.0.0")]
public static class SampleDataProvider
{
    public static IReadOnlyList<Contact> CreateContacts()
    {
        var contacts = new List<Contact>()
        {
            CreateContact("Jesper", "Aaberg", "jesper.aaberg@example.com", "(111) 555-0100", "A. Datum Corporation", "Main St. 4567", "Buffalo", "New York", "98052", "United States"),
            CreateContact("Lori", "Penor", "lori.penor@fabrikam.com", "(111) 555-0104", "Baldwin Museum of Science", "Front St. 3598", "Seattle", "Washington", "12345", "United States"),
            CreateContact("Michael", "Pfeiffer", "michael.pfeiffer@fabrikam.com", "(222) 555-0105", "Blue Yonder Airlines", "Front St. 1234", "Seattle", "Washington", "12345", "United States"),
            CreateContact("Terry", "Adams", "terry.adams@adventure-works.com", "(333) 555-0102", "Adventure Works", "Main St. 789", "Buffalo", "New York", "98052", "United States"),
            CreateContact("Miles", "Reid", "miles.reid@adventure-works.com", "(444) 555-0123", "Adventure Works", "22nd St NE 349", "Miami", "Florida", "98052", "United States")
        };
        return contacts;
    }

    private static Contact CreateContact(string firstname, string lastname, string email, string phone, string company, string street, string city, string state, string postalCode, string country)
    {
        var contact = new Contact() { Firstname = firstname, Lastname = lastname, Email = email, Phone = phone, Company = company };
        contact.Address.Street = street;
        contact.Address.City = city;
        contact.Address.State = state;
        contact.Address.PostalCode = postalCode;
        contact.Address.Country = country;
        return contact;
    }
}
