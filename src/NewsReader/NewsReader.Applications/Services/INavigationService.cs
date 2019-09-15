using System.Threading.Tasks;
using Waf.NewsReader.Applications.ViewModels;

namespace Waf.NewsReader.Applications.Services
{
    public interface INavigationService
    {
        Task Navigate(IViewModel viewModel);
    }
}
