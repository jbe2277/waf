using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

public class EmailLayoutViewModel(IEmailLayoutView view) : ViewModel<IEmailLayoutView>(view)
{
    public object? EmailListView { get; set => SetProperty(ref field, value); }

    public object? EmailView { get; set => SetProperty(ref field, value); }
}
