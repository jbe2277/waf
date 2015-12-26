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
        }


        [Required, Display(Name = "POP3 Server")]
        public string Pop3ServerPath
        {
            get { return pop3ServerPath; }
            set { SetProperty(ref pop3ServerPath, value); }
        }

        public UserCredits Pop3UserCredits
        {
            get { return pop3UserCredits; }
        }

        [Required, Display(Name = "SMTP Server")]
        public string SmtpServerPath
        {
            get { return smtpServerPath; }
            set { SetProperty(ref smtpServerPath, value); }
        }

        public UserCredits SmtpUserCredits
        {
            get { return smtpUserCredits; }
        }


        public override EmailAccountSettings Clone()
        {
            return new Pop3Settings() 
            { 
                pop3UserCredits = this.pop3UserCredits.Clone(), 
                smtpUserCredits = this.smtpUserCredits.Clone(), 
                pop3ServerPath = this.pop3ServerPath, 
                smtpServerPath = this.smtpServerPath 
            };
        }
    }
}
