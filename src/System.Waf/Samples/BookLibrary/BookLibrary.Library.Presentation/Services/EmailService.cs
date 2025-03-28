﻿using System.Diagnostics;
using System.Waf.Applications.Services;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Presentation.Properties;

namespace Waf.BookLibrary.Library.Presentation.Services;

internal sealed class EmailService : IEmailService
{
    private readonly IMessageService messageService;
    private readonly IShellService shellService;

    public EmailService(IMessageService messageService, IShellService shellService)
    {
        this.messageService = messageService;
        this.shellService = shellService;
    }

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
