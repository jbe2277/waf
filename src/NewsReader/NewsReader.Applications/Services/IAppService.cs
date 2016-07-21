using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IAppService
    {
        Task DelayIdleAsync();
    }
}
