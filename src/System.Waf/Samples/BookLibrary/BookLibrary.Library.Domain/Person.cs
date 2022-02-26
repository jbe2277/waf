using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Waf.BookLibrary.Library.Domain.Properties;

namespace Waf.BookLibrary.Library.Domain;

[DebuggerDisplay("Person: {Firstname} {Lastname}")]
public class Person : ValidatableModel, IFormattable
{
    private string firstname;
    private string lastname;
    private string? email;

    public Person()
    {
        Id = Guid.NewGuid();
        firstname = "";
        lastname = "";
    }

    public Guid Id { get; private set; }

    [Required(ErrorMessageResourceName = nameof(Resources.FirstnameMandatory), ErrorMessageResourceType = typeof(Resources))]
    [StringLength(30, ErrorMessageResourceName = nameof(Resources.FirstnameMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string Firstname
    {
        get => firstname;
        set => SetPropertyAndValidate(ref firstname, value);
    }

    [Required(ErrorMessageResourceName = nameof(Resources.LastnameMandatory), ErrorMessageResourceType = typeof(Resources))]
    [StringLength(30, ErrorMessageResourceName = nameof(Resources.LastnameMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string Lastname
    {
        get => lastname;
        set => SetPropertyAndValidate(ref lastname, value);
    }

    [StringLength(100, ErrorMessageResourceName = nameof(Resources.EmailMaxLength), ErrorMessageResourceType = typeof(Resources))]
    [EmailAddress(ErrorMessageResourceName = nameof(Resources.EmailInvalid), ErrorMessageResourceType = typeof(Resources))]
    public string? Email
    {
        get => email;
        set => SetPropertyAndValidate(ref email, value == "" ? null : value);
    }

    public string ToString(string? format, IFormatProvider? formatProvider) => string.Format(formatProvider, Resources.PersonToString, Firstname, Lastname);
}
