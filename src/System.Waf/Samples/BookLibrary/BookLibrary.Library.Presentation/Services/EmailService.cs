using System.Diagnostics;
using System.Waf.Applications.Services;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Presentation.Properties;

namespace Waf.BookLibrary.Library.Presentation.Services;

internal sealed class EmailService(IMessageService messageService, IShellService shellService) : IEmailService
{
    private readonly IMessageService messageService = messageService;
    private readonly IShellService shellService = shellService;

    public void CreateNewEmail(string toEmailAddress)
    {
        try
        {
            Log.Default.Info("Create email for: {0}", toEmailAddress);
            Process.Start(new ProcessStartInfo("mailto:" + toEmailAddress) { UseShellExecute = true });
        }
        catch (Exception e)
        {
            Log.Default.Error(e, "Cannot create a new email");
            messageService.ShowError(shellService.ShellView, Resources.CreateEmailError);
        }
    }
}
