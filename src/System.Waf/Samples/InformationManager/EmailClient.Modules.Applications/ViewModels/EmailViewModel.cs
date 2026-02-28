using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

public class EmailViewModel(IEmailView view) : ViewModel<IEmailView>(view)
{
    public Email? Email { get; set => SetProperty(ref field, value); }
}
