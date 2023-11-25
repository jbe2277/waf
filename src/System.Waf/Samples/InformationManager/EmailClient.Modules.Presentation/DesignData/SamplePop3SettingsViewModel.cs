using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData;

public class SamplePop3SettingsViewModel : Pop3SettingsViewModel
{
    public SamplePop3SettingsViewModel() : base(new MockPop3SettingsView())
    {
        var pop3Settings = new Pop3Settings() { Pop3ServerPath = "pop3.example.com", SmtpServerPath = "smtp.example.com" };
        pop3Settings.Pop3UserCredits.UserName = "pop3User";
        pop3Settings.SmtpUserCredits.UserName = "smtpUser";
        Model = pop3Settings;
    }


    private sealed class MockPop3SettingsView : IPop3SettingsView
    {
        public object? DataContext { get; set; }
    }
}
