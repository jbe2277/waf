using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings
{
    [DataContract]
    public class Pop3Settings : EmailAccountSettings
    {
        [DataMember] private UserCredits pop3UserCredits;
        [DataMember] private UserCredits smtpUserCredits;
        [DataMember] private string pop3ServerPath;
        [DataMember] private string smtpServerPath;

        public Pop3Settings()
        {
            pop3UserCredits = new UserCredits();
            smtpUserCredits = new UserCredits();
            pop3UserCredits.Validate();
            smtpUserCredits.Validate();
        }

        [Required, Display(Name = "POP3 Server")]
        public string Pop3ServerPath
        {
            get { return pop3ServerPath; }
            set { SetPropertyAndValidate(ref pop3ServerPath, value); }
        }

        public UserCredits Pop3UserCredits => pop3UserCredits;

        [Required, Display(Name = "SMTP Server")]
        public string SmtpServerPath
        {
            get { return smtpServerPath; }
            set { SetPropertyAndValidate(ref smtpServerPath, value); }
        }

        public UserCredits SmtpUserCredits => smtpUserCredits;

        public override EmailAccountSettings Clone()
        {
            var clone = new Pop3Settings() 
            { 
                pop3UserCredits = pop3UserCredits.Clone(), 
                smtpUserCredits = smtpUserCredits.Clone(), 
                pop3ServerPath = pop3ServerPath, 
                smtpServerPath = smtpServerPath 
            };
            clone.Validate();
            return clone;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Validate();
        }
    }
}
