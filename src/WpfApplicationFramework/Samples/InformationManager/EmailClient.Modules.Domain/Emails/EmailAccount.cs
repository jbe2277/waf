using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Waf.InformationManager.Common.Domain;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails
{
    public class EmailAccount : ValidationModel
    {
        [DataMember] private string name;
        [DataMember] private string email;
        [DataMember] private EmailAccountSettings emailAccountSettings;


        [Required, Display(Name = "Name")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        [Required, StringLength(100), Display(Name = "Email Address")]
        [EmailAddress]
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        public EmailAccountSettings EmailAccountSettings
        {
            get { return emailAccountSettings; }
            set { SetProperty(ref emailAccountSettings, value); }
        }


        public virtual EmailAccount Clone()
        {
            return new EmailAccount() 
            { 
                name = this.name, 
                email = this.email, 
                emailAccountSettings = this.emailAccountSettings != null ? this.emailAccountSettings.Clone() : null 
            };
        }
    }
}
