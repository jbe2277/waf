using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.AddressBook.Modules.Domain;

[DataContract]
public class Address : ValidatableModel
{
    [DataMember] private string? street;
    [DataMember] private string? city;
    [DataMember] private string? state;
    [DataMember] private string? postalCode;
    [DataMember] private string? country;

    public string? Street
    {
        get => street;
        set => SetPropertyAndValidate(ref street, value);
    }

    public string? City
    {
        get => city;
        set => SetPropertyAndValidate(ref city, value);
    }

    public string? State
    {
        get => state;
        set => SetPropertyAndValidate(ref state, value);
    }

    public string? PostalCode
    {
        get => postalCode;
        set => SetPropertyAndValidate(ref postalCode, value);
    }

    public string? Country
    {
        get => country;
        set => SetPropertyAndValidate(ref country, value);
    }
}
