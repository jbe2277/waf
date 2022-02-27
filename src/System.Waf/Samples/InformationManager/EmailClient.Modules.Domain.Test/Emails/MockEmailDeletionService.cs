using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Test.InformationManager.EmailClient.Modules.Domain.Emails;

public class MockEmailDeletionService : IEmailDeletionService
{
    public Action<EmailFolder, Email>? DeleteEmailAction { get; set; }

    public void NotifyEmailDeleted(EmailFolder emailFolder, Email email) => DeleteEmailAction?.Invoke(emailFolder, email);
}
