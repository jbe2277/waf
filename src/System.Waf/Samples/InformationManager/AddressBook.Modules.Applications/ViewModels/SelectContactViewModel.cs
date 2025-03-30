using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

public class SelectContactViewModel : ViewModel<ISelectContactView>
{
    public SelectContactViewModel(ISelectContactView view) : base(view)
    {
    }

    public ICommand OkCommand { get; set; } = DelegateCommand.DisabledCommand;

    public object? ContactListView { get; set; }

    public void ShowDialog(object owner) => ViewCore.ShowDialog(owner);

    public void Close() => ViewCore.Close();
}
