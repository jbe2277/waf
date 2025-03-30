using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

public class EmailLayoutViewModel : ViewModel<IEmailLayoutView>
{
    private object? emailListView;
    private object? emailView;

    public EmailLayoutViewModel(IEmailLayoutView view) : base(view)
    {
    }

    public object? EmailListView
    {
        get => emailListView;
        set => SetProperty(ref emailListView, value);
    }

    public object? EmailView
    {
        get => emailView;
        set => SetProperty(ref emailView, value);
    }
}
