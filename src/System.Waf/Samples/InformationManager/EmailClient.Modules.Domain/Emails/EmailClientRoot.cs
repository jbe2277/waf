using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails
{
    [DataContract]
    public class EmailClientRoot : ValidatableModel, IEmailDeletionService
    {
        [DataMember] private readonly ObservableCollection<EmailAccount> emailAccounts;
        [DataMember] private readonly EmailFolder inbox;
        [DataMember] private readonly EmailFolder outbox;
        [DataMember] private readonly EmailFolder sent;
        [DataMember] private readonly EmailFolder drafts;
        [DataMember] private readonly EmailFolder deleted;

        public EmailClientRoot()
        {
            emailAccounts = new ObservableCollection<EmailAccount>();
            inbox = new EmailFolder();
            outbox = new EmailFolder();
            sent = new EmailFolder();
            drafts = new EmailFolder();
            deleted = new EmailFolder();
            Initialize();
        }

        public IReadOnlyList<EmailAccount> EmailAccounts => emailAccounts;

        public EmailFolder Inbox => inbox;

        public EmailFolder Outbox => outbox;

        public EmailFolder Sent => sent;

        public EmailFolder Drafts => drafts;

        public EmailFolder Deleted => deleted;

        public void AddEmailAccount(EmailAccount emailAccount)
        {
            emailAccounts.Add(emailAccount);
        }

        public void RemoveEmailAccount(EmailAccount emailAccount)
        {
            emailAccounts.Remove(emailAccount);
        }

        public void ReplaceEmailAccount(EmailAccount oldEmailAccount, EmailAccount newEmailAccount)
        {
            int index = emailAccounts.IndexOf(oldEmailAccount);
            emailAccounts[index] = newEmailAccount;
        }

        public void NotifyEmailDeleted(EmailFolder emailFolder, Email email)
        {
            if (emailFolder != Deleted)
            {
                Deleted.AddEmail(email);    
            }
        }

        private void Initialize()
        {
            inbox.EmailDeletionService = this;
            outbox.EmailDeletionService = this;
            sent.EmailDeletionService = this;
            drafts.EmailDeletionService = this;
            deleted.EmailDeletionService = this;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Initialize();
        }
    }
}
