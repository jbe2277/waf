using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Waf.InformationManager.Common.Domain;

namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails
{
    [DataContract]
    public class EmailClientRoot : ValidationModel, IEmailDeletionService
    {
        [DataMember] private readonly ObservableCollection<EmailAccount> emailAccounts;
        [DataMember] private readonly EmailFolder inbox;
        [DataMember] private readonly EmailFolder outbox;
        [DataMember] private readonly EmailFolder sent;
        [DataMember] private readonly EmailFolder drafts;
        [DataMember] private readonly EmailFolder deleted;


        public EmailClientRoot()
        {
            this.emailAccounts = new ObservableCollection<EmailAccount>();
            this.inbox = new EmailFolder();
            this.outbox = new EmailFolder();
            this.sent = new EmailFolder();
            this.drafts = new EmailFolder();
            this.deleted = new EmailFolder();
            Initialize();
        }


        public IReadOnlyList<EmailAccount> EmailAccounts { get { return emailAccounts; } }

        public EmailFolder Inbox { get { return inbox; } }

        public EmailFolder Outbox { get { return outbox; } }

        public EmailFolder Sent { get { return sent; } }

        public EmailFolder Drafts { get { return drafts; } }

        public EmailFolder Deleted { get { return deleted; } }


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
