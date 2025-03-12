using System.Windows;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Presentation.Views;

public partial class SaveChangesWindow : ISaveChangesView
{
    public SaveChangesWindow()
    {
        InitializeComponent();
    }

    public void ShowDialog(object owner)
    {
        Owner = owner as Window;
        ShowDialog();
    }
}
