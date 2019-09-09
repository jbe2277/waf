using Waf.NewsReader.Applications.Views;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class ShellViewModel : ViewModel<IShellView>
    {
        public ShellViewModel(IShellView view) : base(view)
        {
        }
    }
}
