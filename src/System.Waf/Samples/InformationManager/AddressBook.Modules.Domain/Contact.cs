using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.AddressBook.Modules.Domain;

[DataContract]
public class Contact : ValidatableModel
{
    [DataMember] private readonly Address address;
    [DataMember] private string? firstname;
    [DataMember] private string? lastname;
    [DataMember] private string? company;
    [DataMember] private string? email;
    [DataMember] private string? phone;

    public Contact()
    {
        address = new Address();
        address.Validate();
    }

    [Required]
    public string? Firstname
    {
        get => firstname;
        set => SetPropertyAndValidate(ref firstname, value);
    }

    public string? Lastname
    {
        get => lastname;
        set => SetPropertyAndValidate(ref lastname, value);
    }

    public string? Company
    {
        get => company;
        set => SetPropertyAndValidate(ref company, value);
    }

    [EmailAddress]
    public string? Email
    {
        get => email;
        set => SetPropertyAndValidate(ref email, value);
    }

    public string? Phone
    {
        get => phone;
        set => SetPropertyAndValidate(ref phone, value);
    }

    public Address Address => address;

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context) => Address.Validate();
}
