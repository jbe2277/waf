using System.Waf.Applications;
using System.Windows;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views;

public partial class EmailAccountsWindow : IEmailAccountsView
{
    private readonly Lazy<EmailAccountsViewModel> viewModel;

    public EmailAccountsWindow()
    {
        InitializeComponent();
        viewModel = new(() => ViewHelper.GetViewModel<EmailAccountsViewModel>(this)!);
    }

    public void ShowDialog(object owner)
    {
        Owner = owner as Window;
        ShowDialog();
    }

    private void EmailAccountsGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is FrameworkElement element && element.DataContext is EmailAccount)
        {
            viewModel.Value.EditAccountCommand.Execute(null);
        }
    }
}
