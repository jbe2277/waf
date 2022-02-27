using System.Waf.Applications;

namespace Waf.InformationManager.AddressBook.Modules.Applications.Views;

public interface ISelectContactView : IView
{
    void ShowDialog(object owner);

    void Close();
}
