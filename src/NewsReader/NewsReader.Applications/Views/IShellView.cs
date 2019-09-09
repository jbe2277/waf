using System.Threading.Tasks;
using System.Waf.Applications;

namespace Waf.NewsReader.Applications.Views
{
    public interface IShellView : IView
    {
        Task NavigateAsync(object page);
    }
}
