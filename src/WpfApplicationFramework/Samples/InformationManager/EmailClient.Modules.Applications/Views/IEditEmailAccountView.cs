using System.Waf.Applications;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Views
{
    public interface IEditEmailAccountView : IView
    {
        void ShowDialog(object owner);

        void Close();
    }
}
