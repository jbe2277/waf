using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

public class EmailListViewModel(IEmailListView view) : ViewModel<IEmailListView>(view)
{
    public IReadOnlyList<Email> Emails { get; set; } = null!;

    public Email? SelectedEmail { get; set => SetProperty(ref field, value); }

    public ICommand DeleteEmailCommand { get; set; } = DelegateCommand.DisabledCommand;

    public string FilterText { get; set => SetProperty(ref field, value); } = "";

    public void FocusItem() => ViewCore.FocusItem();

    public bool Filter(Email email)
    {
        return string.IsNullOrEmpty(FilterText)
            || (!string.IsNullOrEmpty(email.Title) && email.Title.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase))
            || (!string.IsNullOrEmpty(email.From) && email.From.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase))
            || email.To.Any(x => !string.IsNullOrEmpty(x) && x.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase));
    }
}
