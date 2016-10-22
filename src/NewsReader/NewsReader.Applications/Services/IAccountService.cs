using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IAccountService : IAccountInfoService
    {
        void SignIn();

        Task SignOutAsync();
    }
}
