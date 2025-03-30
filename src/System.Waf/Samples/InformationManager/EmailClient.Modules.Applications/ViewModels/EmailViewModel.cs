using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

public class EmailViewModel : ViewModel<IEmailView>
{
    private Email? email;

    public EmailViewModel(IEmailView view) : base(view)
    {
    }

    public Email? Email
    {
        get => email;
        set => SetProperty(ref email, value);
    }
}
