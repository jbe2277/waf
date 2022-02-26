using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails
{
    [DataContract]
    public class EmailFolder : ValidatableModel
    {
        [DataMember] private readonly ObservableCollection<Email> emails;
        private ReadOnlyObservableList<Email>? readOnlyEmails;

        public EmailFolder()
        {
            emails = new ObservableCollection<Email>();
        }

        public IReadOnlyObservableList<Email> Emails => readOnlyEmails ??= new ReadOnlyObservableList<Email>(emails);

        internal IEmailDeletionService EmailDeletionService { get; set; } = null!;

        public void AddEmail(Email email) => emails.Add(email);

        public void RemoveEmail(Email email)
        {
            emails.Remove(email);
            EmailDeletionService.NotifyEmailDeleted(this, email);
        }
    }
}
