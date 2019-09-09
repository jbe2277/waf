using System.Waf.Applications;
using Waf.NewsReader.Applications.Views;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class ShellViewModel : ViewModelCore<IShellView>
    {
        public ShellViewModel(IShellView view) : base(view)
        {
        }
    }
}
