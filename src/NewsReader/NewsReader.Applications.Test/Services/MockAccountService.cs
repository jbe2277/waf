using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;
using Jbe.NewsReader.Domain;
using System.Waf.Foundation;

namespace Test.NewsReader.Applications.Services
{
    [Export, Export(typeof(IAccountService)), Export(typeof(IAccountInfoService)), Shared]
    public class MockAccountService : Model, IAccountService
    {
        public UserAccount CurrentAccount { get; set; }

        public Func<Task<string>> GetTokenAsyncStub { get; set; }

        public Task<string> GetTokenAsync()
        {
            return GetTokenAsyncStub?.Invoke() ?? Task.FromResult((string)null);
        }

        public Func<Task> InitializeAsyncStub { get; set; }

        public Task InitializeAsync()
        {
            return InitializeAsyncStub?.Invoke() ?? Task.FromResult((object)null);
        }

        public Action<Action<Task<UserAccount>>> SignInStub { get; set; }

        public void SignIn(Action<Task<UserAccount>> signInStarted)
        {
            SignInStub?.Invoke(signInStarted);
        }

        public Func<Task> SignOutAsyncStub { get; set; }

        public Task SignOutAsync()
        {
            return SignOutAsyncStub?.Invoke() ?? Task.FromResult((object)null);
        }
    }
}
