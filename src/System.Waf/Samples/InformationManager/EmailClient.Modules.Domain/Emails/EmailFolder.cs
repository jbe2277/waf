using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Waf.Foundation;
using Waf.InformationManager.Common.Domain;

namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails
{
    [DataContract]
    public class EmailFolder : ValidationModel
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
    }
}
