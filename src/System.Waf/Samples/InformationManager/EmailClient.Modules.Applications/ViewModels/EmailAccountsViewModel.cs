using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

public class EmailAccountsViewModel(IEmailAccountsView view) : ViewModel<IEmailAccountsView>(view)
{
    public EmailClientRoot EmailClientRoot { get; set; } = null!;

    public ICommand NewAccountCommand { get; set; } = null!;

    public ICommand RemoveAccountCommand { get; set; } = null!;

    public ICommand EditAccountCommand { get; set; } = null!;

    public EmailAccount? SelectedEmailAccount { get; set => SetProperty(ref field, value); }

    public void ShowDialog(object owner) => ViewCore.ShowDialog(owner);
}
