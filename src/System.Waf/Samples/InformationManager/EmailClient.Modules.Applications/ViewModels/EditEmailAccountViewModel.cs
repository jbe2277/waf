using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

[Export, PartCreationPolicy(CreationPolicy.NonShared)]
public class EditEmailAccountViewModel : ViewModel<IEditEmailAccountView>
{
    private object contentView = null!;
    private bool isValid = true;
    private bool isLastPage;

    [ImportingConstructor]
    public EditEmailAccountViewModel(IEditEmailAccountView view) : base(view)
    {
    }

    public ICommand BackCommand { get; set; } = null!;

    public ICommand NextCommand { get; set; } = null!;

    public object ContentView
    {
        get => contentView;
        set => SetProperty(ref contentView, value);
    }

    public bool IsValid
    {
        get => isValid;
        set => SetProperty(ref isValid, value);
    }

    public bool IsLastPage
    {
        get => isLastPage;
        set => SetProperty(ref isLastPage, value);
    }

    public void ShowDialog(object owner) => ViewCore.ShowDialog(owner);

    public void Close() => ViewCore.Close();
}
