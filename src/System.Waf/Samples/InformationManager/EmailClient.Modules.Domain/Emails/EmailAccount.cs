using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Waf.Foundation;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails;

public class EmailAccount : ValidatableModel
{
    [DataMember] private string? name;
    [DataMember] private string? email;
    [DataMember] private EmailAccountSettings? emailAccountSettings;

    [Required, Display(Name = "Name")]
    public string? Name
    {
        get => name;
        set => SetPropertyAndValidate(ref name, value);
    }

    [Required, StringLength(100), Display(Name = "Email Address")]
    [EmailAddress]
    public string? Email
    {
        get => email;
        set => SetPropertyAndValidate(ref email, value);
    }

    public EmailAccountSettings? EmailAccountSettings
    {
        get => emailAccountSettings;
        set => SetPropertyAndValidate(ref emailAccountSettings, value);
    }

    public virtual EmailAccount Clone()
    {
        var clone = new EmailAccount()
        {
            name = name,
            email = email,
            emailAccountSettings = emailAccountSettings?.Clone()
        };
        clone.Validate();
        return clone;
    }

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context) => Validate();
}
