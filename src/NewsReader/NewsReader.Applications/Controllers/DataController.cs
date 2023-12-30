using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers;

internal sealed class DataController
{
    private readonly AppSettings appSettings;
    private readonly IDataService dataService;
    private readonly INetworkInfoService networkInfoService;
    private readonly IWebStorageService webStorageService;
    private readonly IMessageService messageService;
    private readonly AsyncDelegateCommand signInCommand;
    private readonly AsyncDelegateCommand signOutCommand;
    private readonly TaskCompletionSource<FeedManager> loadCompletion = new();
    private bool isInitialized;
    private bool isInSync;

    public DataController(ISettingsService settingsService, IDataService dataService, INetworkInfoService networkInfoService,
        IWebStorageService webStorageService, IMessageService messageService)
    {
        appSettings = settingsService.Get<AppSettings>();
        this.dataService = dataService;
        this.networkInfoService = networkInfoService;
        this.webStorageService = webStorageService;
        this.messageService = messageService;
        signInCommand = new(SignIn, () => isInitialized);
        signOutCommand = new(SignOutAsync);
        webStorageService.PropertyChanged += WebStorageServicePropertyChanged;
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
        FeedManager? feedManager = null;
        try
        {
            await Task.Run(() => { feedManager = dataService.Load<FeedManager>() ?? new FeedManager(); });
        }
        catch (Exception ex)
        {
            // Better to forget the settings (data loss) as to never start the app again
            Log.Default.TrackError(ex, "DataController.Load");
            feedManager = new FeedManager();
        }
        loadCompletion.SetResult(feedManager!);
        return feedManager!;
    }

    public Task Update() => DownloadAndMerge();

    public Task Save()
    {
        if (!loadCompletion.Task.IsCompleted) return Task.CompletedTask;
        var feedManager = loadCompletion.Task.GetAwaiter().GetResult();
        dataService.Save(feedManager);
        return Upload();
    }

    private async Task SignIn()
    {
        try
        {
            await webStorageService.SignIn();
        }
        catch (Exception ex)
        {
            Log.Default.TrackError(ex, "Account sign in failed");
            await messageService.ShowMessage(Resources.SignInError, ex.Message);
        }
    }

    private Task SignOutAsync() => webStorageService.SignOut();

    private async Task DownloadAndMerge()
    {
        if (webStorageService.CurrentAccount == null || !networkInfoService.InternetAccess) return;

        FeedManager? feedManagerFromWeb = null;
        try
        {
            var (stream, cTag) = await webStorageService.DownloadFile(appSettings.WebStorageCTag);
            if (!string.IsNullOrEmpty(cTag))
            {
                appSettings.WebStorageCTag = cTag;
                feedManagerFromWeb = dataService.Load<FeedManager>(stream);
            }
            else
            {
                isInSync = true;  // We are in-sync when no file exists on web storage.
            }
        }
        catch (Exception ex)
        {
            Log.Default.TrackError(ex, "Download failed");
            messageService.ShowMessage(Resources.SynchronizationDownloadError, ex.Message).NoWait();
        }

        if (feedManagerFromWeb != null)
        {
            var originalFeedManager = await loadCompletion.Task;
            originalFeedManager.Merge(feedManagerFromWeb);
            isInSync = true;
        }
    }

    private async Task Upload()
    {
        if (!isInSync || webStorageService.CurrentAccount == null || !networkInfoService.InternetAccess) return;
        try
        {
            var dataFileHash = dataService.GetHash();
            if (dataFileHash != appSettings.LastUploadedFileHash)
            {
                var cTag = await webStorageService.UploadFile(dataService.GetReadStream()).ConfigureAwait(false);
                appSettings.WebStorageCTag = cTag;
                appSettings.LastUploadedFileHash = dataFileHash;
            }
        }
        catch (Exception ex)
        {
            Log.Default.TrackError(ex, "Upload failed");
        }
    }

    private void WebStorageServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(webStorageService.CurrentAccount)) DownloadAndMerge().NoWait();
    }
}
