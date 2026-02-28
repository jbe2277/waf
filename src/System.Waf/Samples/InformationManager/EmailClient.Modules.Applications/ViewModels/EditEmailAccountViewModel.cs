using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

public class EditEmailAccountViewModel(IEditEmailAccountView view) : ViewModel<IEditEmailAccountView>(view)
{
    public ICommand BackCommand { get; set; } = null!;

    public ICommand NextCommand { get; set; } = null!;

    public object ContentView { get; set => SetProperty(ref field, value); } = null!;

    public bool IsValid { get; set => SetProperty(ref field, value); } = true;

    public bool IsLastPage { get; set => SetProperty(ref field, value); }

    public void ShowDialog(object owner) => ViewCore.ShowDialog(owner);

    public void Close() => ViewCore.Close();
}
