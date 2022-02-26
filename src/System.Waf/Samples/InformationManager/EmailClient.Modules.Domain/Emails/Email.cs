using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails
{
    [DataContract]
    public class Email : ValidatableModel, IValidatableObject
    {
        private static readonly EmailAddressAttribute emailAddress = new();

        [DataMember] private EmailType emailType;
        [DataMember] private string title = "";
        [DataMember] private string from = "";
        [DataMember] private IEnumerable<string> to;
        [DataMember] private IEnumerable<string> cc;
        [DataMember] private IEnumerable<string> bcc;
        [DataMember] private DateTime sent;
        [DataMember] private string? message;

        public Email()
        {
            to = Array.Empty<string>();
            cc = Array.Empty<string>();
            bcc = Array.Empty<string>();
        }

        public EmailType EmailType
        {
            get => emailType;
            set => SetPropertyAndValidate(ref emailType, value);
        }

        [StringLength(255), DisplayName("Title")]
        public string Title
        {
            get => title;
            set => SetPropertyAndValidate(ref title, value);
        }

        [StringLength(255), DisplayName("From")]
        public string From
        {
            get => from;
            set => SetPropertyAndValidate(ref from, value);
        }

        public IEnumerable<string> To
        {
            get => to;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (to == value || to.SequenceEqual(value)) return;
                to = value;
                Validate();
                RaisePropertyChanged();
            }
        }

        public IEnumerable<string> CC
        {
            get => cc;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (cc == value || cc.SequenceEqual(value)) return;
                cc = value;
                Validate();
                RaisePropertyChanged();
            }
        }

        public IEnumerable<string> Bcc
        {
            get => bcc;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (bcc == value || bcc.SequenceEqual(value)) return;
                bcc = value;
                Validate();
                RaisePropertyChanged();
            }
        }

        public DateTime Sent
        {
            get => sent;
            set => SetPropertyAndValidate(ref sent, value);
        }

        public string? Message
        {
            get => message;
            set => SetPropertyAndValidate(ref message, value);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            foreach (var email in To) { ValidateEmail(validationResults, email, nameof(To), "To"); }
            foreach (var email in CC) { ValidateEmail(validationResults, email, nameof(CC), "CC"); }
            foreach (var email in Bcc) { ValidateEmail(validationResults, email, nameof(Bcc), "BCC"); }
            if (!To.Any() && !CC.Any() && !Bcc.Any())
            {
                validationResults.Add(new ValidationResult("This email doesn't define a recipient.", new[] { nameof(To) }));
            }
            return validationResults;
        }

        private static void ValidateEmail(ICollection<ValidationResult> validationResults, string email, string field, string displayName)
        {
            if (!emailAddress.IsValid(email))
            {
                validationResults.Add(new ValidationResult(string.Format(CultureInfo.CurrentCulture, "The email {0} in the {1} field is not valid.", email, displayName), new[] { field }));
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => Validate();
    }
}
