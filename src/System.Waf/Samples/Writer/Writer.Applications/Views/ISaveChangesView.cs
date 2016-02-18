using System.Waf.Applications;

namespace Waf.Writer.Applications.Views
{
    public interface ISaveChangesView : IView
    {
        void ShowDialog(object owner);

        void Close();
    }
}
