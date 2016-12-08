using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Domain;
using System;
using System.ComponentModel;
using System.Composition;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Controllers
{
    [Export, Shared]
    internal class DataController
    {
        private const string salt = "20E1EB34-CF2D-4298-9A95-FACC60759745+74546543-0405-4901-9CFC-E88DFB5BACE5";
        private const uint iterationCount = 5000;
        private const string dataFileName = "feeds.xml";

        private readonly IAppDataService appDataService;
        private readonly IAccountService accountService;
        private readonly IWebStorageService webStorageService;
        private readonly ICryptographicService cryptographicService;
        private readonly IMessageService messageService;
        private readonly IResourceService resourceService;
        private readonly TaskCompletionSource<FeedManager> feedManagerCompletion;
        private bool isInSync;


        [ImportingConstructor]
        public DataController(IAppDataService appDataService, IAccountService accountService, IWebStorageService webStorageService, ICryptographicService cryptographicService,
            IMessageService messageService, IResourceService resourceService)
        {
            this.appDataService = appDataService;
            this.accountService = accountService;
            this.webStorageService = webStorageService;
            this.cryptographicService = cryptographicService;
            this.messageService = messageService;
            this.resourceService = resourceService;
            feedManagerCompletion = new TaskCompletionSource<FeedManager>();
        }


        public void Initialize()
        {
            accountService.PropertyChanged += AccountInfoServicePropertyChanged;
            DownloadAndMerge();
        }

        public void Update()
        {
            DownloadAndMerge();
        }

        public async Task<FeedManager> LoadAsync()
        {
            // NOTE: Load must be called just once! See feedManagerCompletion
            FeedManager feedManager;
            try
            {
                feedManager = await appDataService.LoadCompressedFileAsync<FeedManager>(dataFileName) ?? new FeedManager();
            }
            catch (Exception ex)
            {
                // Better to forget the settings (data loss) as to never start the app again
                Debug.Assert(false, "LoadAsync", ex.ToString());
                feedManager = new FeedManager();
            }
            feedManagerCompletion.SetResult(feedManager);
            return feedManager;
        }

        public async Task SaveAsync()
        {
            if (!feedManagerCompletion.Task.IsCompleted) { return; }

            var feedManager = await feedManagerCompletion.Task;
            await appDataService.SaveCompressedFileAsync(feedManager, dataFileName);
            await UploadAsync();
        }

        private async void DownloadAndMerge()
        {
            if (accountService.CurrentAccount == null) { return; }
            var token = await accountService.GetTokenAsync();
            if (token == null) { return; }

            FeedManager feedManagerFromWeb = null;
            using (var cryptoStream = new MemoryStream())
            {
                try
                {
                    if (await webStorageService.DownloadFileAsync(dataFileName, cryptoStream, token))
                    {
                        cryptoStream.Position = 0;
                        using (var stream = await cryptographicService.DecryptAsync(cryptoStream, accountService.CurrentAccount.Id, salt + accountService.CurrentAccount.Id, iterationCount))
                        {
                            feedManagerFromWeb = appDataService.LoadCompressedFile<FeedManager>(stream, dataFileName);
                        }
                    }
                    else
                    {
                        isInSync = true;  // We are in-sync when no file exists on web storage.
                    }
                }
                catch (Exception ex)
                {
                    messageService.ShowMessage(resourceService.GetString("SynchronizationDownloadError"), ex.Message);
                }
            }

            if (feedManagerFromWeb != null)
            {
                var originalFeedManager = await feedManagerCompletion.Task;
                originalFeedManager.Merge(feedManagerFromWeb);
                isInSync = true;
            }
        }
        
        private async Task UploadAsync()
        {
            // TODO: When the online file is corrupt then the upload does not run anymore.
            if (!isInSync || accountService.CurrentAccount == null) { return; }
            var token = await accountService.GetTokenAsync();
            if (token == null) { return; }

            try
            {
                using (var stream = await appDataService.GetFileStreamForReadAsync(dataFileName))
                using (var cryptoStream = await cryptographicService.EncryptAsync(stream, accountService.CurrentAccount.Id, salt + accountService.CurrentAccount.Id, iterationCount))
                {
                    await webStorageService.UploadFileAsync(cryptoStream, dataFileName, token);
                }
            }
            catch (Exception ex)
            {
                messageService.ShowMessage(resourceService.GetString("SynchronizationUploadError"), ex.Message);
            }
        }

        private void AccountInfoServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(accountService.CurrentAccount))
            {
                DownloadAndMerge();
            }
        }
    }
}
