using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Domain;
using System;
using System.Composition;
using System.Globalization;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.Controllers
{
    [Export, Shared]
    internal class AccountController
    {
        private readonly IAccountService accountService;
        private readonly IMessageService messageService;
        private readonly IResourceService resourceService;
        private readonly DelegateCommand signInCommand;
        private readonly AsyncDelegateCommand signOutCommand;
        private bool signInTaskRunning;


        [ImportingConstructor]
        public AccountController(IAccountService accountService, IMessageService messageService, IResourceService resourceService)
        {
            this.accountService = accountService;
            this.messageService = messageService;
            this.resourceService = resourceService;
            signInCommand = new DelegateCommand(SignIn, CanSignIn);
            signOutCommand = new AsyncDelegateCommand(SignOutAsync);
        }


        public ICommand SignInCommand => signInCommand;

        public ICommand SignOutCommand => signOutCommand;


        public async void Initialize()
        {
            try
            {
                await accountService.InitializeAsync();
            }
            catch (Exception ex)
            {
                await messageService.ShowMessageDialogAsync(resourceService.GetString("SignInError"), ex.Message);
            }
        }

        private bool CanSignIn()
        {
            return !signInTaskRunning;
        }

        private void SignIn()
        {
            accountService.SignIn(SignInStarted);
        }

        private async void SignInStarted(Task<UserAccount> signInTask)
        {
            signInTaskRunning = true;
            signInCommand.RaiseCanExecuteChanged();
            try
            {
                await signInTask;
            }
            catch (Exception ex)
            {
                await messageService.ShowMessageDialogAsync(resourceService.GetString("SignInError"), ex.Message);  
            }
            finally
            {
                signInTaskRunning = false;
                signInCommand.RaiseCanExecuteChanged();
            }
        }

        private Task SignOutAsync()
        {
            return accountService.SignOutAsync();
        }
    }
}
