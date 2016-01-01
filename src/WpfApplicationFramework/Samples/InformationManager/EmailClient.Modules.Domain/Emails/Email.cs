using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using Waf.InformationManager.Common.Domain;

namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails
{
    [DataContract]
    public class Email : ValidationModel, IValidatableObject
    {
        private static readonly EmailAddressAttribute emailAddress = new EmailAddressAttribute();

        [DataMember] private EmailType emailType;
        [DataMember] private string title = "";
        [DataMember] private string from = "";
        [DataMember] private IEnumerable<string> to;
        [DataMember] private IEnumerable<string> cc;
        [DataMember] private IEnumerable<string> bcc;
        [DataMember] private DateTime sent;
        [DataMember] private string message;


        public Email()
        {
            to = new string[] { };
            cc = new string[] { };
            bcc = new string[] { };
        }


        public EmailType EmailType
        {
            get { return emailType; }
            set { SetProperty(ref emailType, value); }
        }

        [StringLength(255), DisplayName("Title")]
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        [StringLength(255), DisplayName("From")]
        public string From
        {
            get { return from; }
            set { SetProperty(ref from, value); }
        }

        public IEnumerable<string> To
        {
            get { return to; }
            set
            {
                if (to != value)
                {
                    if (value == null) { throw new ArgumentNullException("value"); }
                    to = value;
                    RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<string> CC
        {
            get { return cc; }
            set
            {
                if (cc != value)
                {
                    if (value == null) { throw new ArgumentNullException("value"); }
                    cc = value;
                    RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<string> Bcc
        {
            get { return bcc; }
            set
            {
                if (bcc != value)
                {
                    if (value == null) { throw new ArgumentNullException("value"); }
                    bcc = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime Sent
        {
            get { return sent; }
            set { SetProperty(ref sent, value); }
        }

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            
            foreach (string email in To) { ValidateEmail(validationResults, email, nameof(To), "To"); }
            foreach (string email in CC) { ValidateEmail(validationResults, email, nameof(CC), "CC"); }
            foreach (string email in Bcc) { ValidateEmail(validationResults, email, nameof(Bcc), "BCC"); }

            if (!To.Any() && !CC.Any() && !Bcc.Any())
            {
                validationResults.Add(new ValidationResult("This email doesn't define a recipient.", new[] { nameof(To), nameof(CC), nameof(Bcc) }));
            }

            return validationResults;
        }

        private static void ValidateEmail(ICollection<ValidationResult> validationResults, string email, string field, string displayName)
        {
            if (!emailAddress.IsValid(email))
            {
                validationResults.Add(new ValidationResult(string.Format(CultureInfo.CurrentCulture, 
                    "The email {0} in the {1} field is not valid.", email, displayName), new[] { field }));
            }
        }
    }
}
