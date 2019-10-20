using Microsoft.AppCenter.Crashes;
using System;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers
{
    internal class DataController
    {
        private readonly IDataService dataService;
        private readonly IWebStorageService webStorageService;
        private readonly IMessageService messageService;
        private readonly AsyncDelegateCommand signInCommand;
        private readonly AsyncDelegateCommand signOutCommand;
        private readonly TaskCompletionSource<FeedManager> loadCompletion;
        private bool isInitialized;

        public DataController(IDataService dataService, IWebStorageService webStorageService, IMessageService messageService)
        {
            this.dataService = dataService;
            this.webStorageService = webStorageService;
            this.messageService = messageService;
            signInCommand = new AsyncDelegateCommand(SignIn, () => isInitialized);
            signOutCommand = new AsyncDelegateCommand(SignOutAsync);
            loadCompletion = new TaskCompletionSource<FeedManager>();
        }

        public ICommand SignInCommand => signInCommand;

        public ICommand SignOutCommand => signOutCommand;

        public async void Initialize()
        {
            await webStorageService.TrySilentSignIn();
            isInitialized = true;
        }

        public async Task<FeedManager> Load()
        {
            FeedManager feedManager = null;
            try
            {
                await Task.Run(() =>
                {
                    feedManager = dataService.Load<FeedManager>() ?? new FeedManager();
                });
            }
            catch (Exception ex)
            {
                // Better to forget the settings (data loss) as to never start the app again
                Log.Default.Error(ex, "DataController.Load");
                Crashes.TrackError(ex);
                feedManager = new FeedManager();
            }
            loadCompletion.SetResult(feedManager);
            return feedManager;
        }

        public Task Update()
        {
            // TODO: Download and update
            return Task.CompletedTask;
        }

        public Task Save()
        {
            if (!loadCompletion.Task.IsCompleted) return Task.CompletedTask;
            var feedManager = loadCompletion.Task.GetAwaiter().GetResult();
            dataService.Save(feedManager);
            // TODO: Upload here
            return Task.CompletedTask;
        }

        private async Task SignIn()
        {
            try
            {
                await webStorageService.SignIn();
            }
            catch (Exception ex)
            {
                Log.Default.Error(ex, "Account sign in failed.");
                Crashes.TrackError(ex);
                await messageService.ShowMessage(Resources.SignInError, ex.Message);
            }
        }

        private Task SignOutAsync()
        {
            return webStorageService.SignOut();
        }
    }
}
