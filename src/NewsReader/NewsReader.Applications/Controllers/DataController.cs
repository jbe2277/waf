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
        private const string dataFileName = "feeds.xml";

        private readonly IAppDataService appDataService;
        private readonly IAccountService accountService;
        private readonly IWebStorageService webStorageService;
        private FeedManager feedManager;


        [ImportingConstructor]
        public DataController(IAppDataService appDataService, IAccountService accountService, IWebStorageService webStorageService)
        {
            this.appDataService = appDataService;
            this.accountService = accountService;
            this.webStorageService = webStorageService;
        }


        public void Initialize()
        {
            accountService.PropertyChanged += AccountInfoServicePropertyChanged;
            DownloadAndMerge();
        }

        public async Task<FeedManager> LoadAsync()
        {
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
            return feedManager;
        }

        public async Task SaveAsync()
        {
            await appDataService.SaveCompressedFileAsync(feedManager, dataFileName);
            await UploadAsync();
        }

        private async void DownloadAndMerge()
        {
            if (accountService.CurrentAccount == null) { return; }
            var token = await accountService.GetTokenAsync();
            if (token == null) { return; }

            using (var stream = new MemoryStream())
            {
                try
                {
                    if (await webStorageService.DownloadFileAsync(dataFileName, stream, token))
                    {
                        stream.Position = 0;
                        var feedManagerFromWeb = appDataService.LoadCompressedFile<FeedManager>(stream, dataFileName);
                        
                        // TODO: Merge data into current feedManager
                    }
                }
                catch (Exception)
                {
                    // TODO: Download has failed - or another error; how to handle this???
                }
            }
        }
        
        private async Task UploadAsync()
        {
            if (accountService.CurrentAccount == null) { return; }
            var token = await accountService.GetTokenAsync();
            if (token == null) { return; }

            try
            {
                using (var stream = await appDataService.GetFileStreamForReadAsync(dataFileName))
                {
                    await webStorageService.UploadFileAsync(stream, dataFileName, token);
                }
            }
            catch (Exception)
            {
                // TODO: Upload has failed; how to handle this???
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
