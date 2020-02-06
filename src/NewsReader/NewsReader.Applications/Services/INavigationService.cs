using System.Threading.Tasks;
using System.Waf.Applications;

namespace Waf.NewsReader.Applications.Services
{
    public interface INavigationService
    {
        Task Navigate(IViewModelCore viewModel);

        Task NavigateBack();
    }
}
