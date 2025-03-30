using System.Windows;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views;

public partial class EditEmailAccountWindow : IEditEmailAccountView
{
    public EditEmailAccountWindow()
    {
        InitializeComponent();
    }

    public void ShowDialog(object owner)
    {
        Owner = owner as Window;
        ShowDialog();
    }
}
