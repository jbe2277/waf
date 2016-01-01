using System.Waf.Applications;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Views
{
    public interface INewEmailView : IView
    {
        void Show(object owner);

        void Close();
    }
}
