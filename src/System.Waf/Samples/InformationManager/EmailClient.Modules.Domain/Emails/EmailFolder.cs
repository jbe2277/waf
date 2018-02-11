using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails
{
    [DataContract]
    public class EmailFolder : ValidatableModel, IValidatableObject
    {
        [DataMember] private readonly ObservableCollection<Email> emails;

        private ReadOnlyObservableList<Email> readOnlyEmails;


        public EmailFolder()
        {
            emails = new ObservableCollection<Email>();
        }


        public IReadOnlyObservableList<Email> Emails => readOnlyEmails ?? (readOnlyEmails = new ReadOnlyObservableList<Email>(emails));

        internal IEmailDeletionService EmailDeletionService { get; set; }


        public void AddEmail(Email email)
        {
            emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            emails.Remove(email);
            EmailDeletionService.NotifyEmailDeleted(this, email);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var email in emails) email.Validate();
            return Array.Empty<ValidationResult>();
        }
    }
}
