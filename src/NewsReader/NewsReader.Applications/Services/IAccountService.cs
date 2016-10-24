using Jbe.NewsReader.Domain;
using System;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IAccountService : IAccountInfoService
    {
        Task InitializeAsync();

        void SignIn(Action<Task<UserAccount>> signInStarted);

        Task SignOutAsync();

        Task<string> GetTokenAsync();
    }
}
