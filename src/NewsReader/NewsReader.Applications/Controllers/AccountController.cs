using Jbe.NewsReader.Applications.Services;
using System.Composition;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.Controllers
{
    [Export, Shared]
    internal class AccountController
    {
        private readonly IAccountService accountService;
        private readonly DelegateCommand signInCommand;
        private readonly AsyncDelegateCommand signOutCommand;


        [ImportingConstructor]
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
            signInCommand = new DelegateCommand(SignIn);
            signOutCommand = new AsyncDelegateCommand(SignOutAsync);
        }


        public ICommand SignInCommand => signInCommand;

        public ICommand SignOutCommand => signOutCommand;


        private void SignIn()
        {
            accountService.SignIn();
        }

        private Task SignOutAsync()
        {
            return accountService.SignOutAsync();
        }
    }
}
