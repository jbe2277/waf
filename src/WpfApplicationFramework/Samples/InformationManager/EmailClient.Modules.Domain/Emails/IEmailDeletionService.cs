namespace Waf.InformationManager.EmailClient.Modules.Domain.Emails
{
    public interface IEmailDeletionService
    {
        void NotifyEmailDeleted(EmailFolder emailFolder, Email email);
    }
}
