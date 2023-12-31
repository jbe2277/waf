using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.Kiota.Abstractions.Authentication;
using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services;

// Add WebStorageService.keys.cs file with:
//partial class WebStorageService
//{
//    static partial void GetApplicationId(ref string applicationId)
//    {
//        applicationId = "{key}";
//    }
//}

internal sealed partial class WebStorageService : Model, IWebStorageService
{
    private const string dataFileName = "data.zip";
    private static readonly string[] scopes = [ "User.Read", "Files.ReadWrite.AppFolder" ];

    private readonly IPublicClientApplication? publicClient;
    private GraphServiceClient? graphClient;
    private UserAccount? currentAccount;

    public WebStorageService()
    {
        string? appId = null;
        GetApplicationId(ref appId);
        if (appId != null)
        {
            var builder = PublicClientApplicationBuilder.Create(appId);
            builder.WithRedirectUri("wafe5b8cee6-8ba0-46c5-96ef-a3c8a1e2bb26://auth");
#if ANDROID
            builder.WithParentActivityOrWindow(() => Platform.CurrentActivity);
#elif IOS
            builder.WithIosKeychainSecurityGroup(Foundation.NSBundle.MainBundle.BundleIdentifier);
#endif
            publicClient = builder.Build();
        }
    }

    public UserAccount? CurrentAccount
    {
        get => currentAccount;
        private set => SetProperty(ref currentAccount, value);
    }

#if WINDOWS
    private bool cacheInitialized;
#endif
    public async Task<bool> TrySilentSignIn()
    {
#if WINDOWS
        if (!cacheInitialized && publicClient is not null)
        {
            cacheInitialized = true;
            var storageProperties = new StorageCreationPropertiesBuilder("msal.dat", FileSystem.CacheDirectory).Build();
            var cacheHelper = await MsalCacheHelper.CreateAsync(storageProperties);
            cacheHelper.RegisterCache(publicClient.UserTokenCache);
        }
#endif

        try
        {
            var accessToken = await TrySilentSignInCore();
            if (!string.IsNullOrEmpty(accessToken))
            {
                await InitGraphClient();
                return true;
            }
        }
        catch (Exception ex)
        {
            Log.Default.TrackError(ex, "Silent login failed");   // Ignore (e.g. no internet access)
        }
        return false;
    }

    public async Task SignIn()
    {
        var accessToken = await TrySilentSignInCore();
        if (publicClient != null && string.IsNullOrEmpty(accessToken))
        {
            try
            {
                await publicClient.AcquireTokenInteractive(scopes).ExecuteAsync();
            }
            catch (MsalClientException ex) when (ex.ErrorCode == MsalError.AuthenticationCanceledError)
            {
                throw new OperationCanceledException(ex.ErrorCode);
            }
        }
        await InitGraphClient();
    }

    public async Task SignOut()
    {
        CurrentAccount = null;
        graphClient = null;
        if (publicClient != null)
        {
            var accounts = await publicClient.GetAccountsAsync().ConfigureAwait(false);
            await Task.WhenAll(accounts.Select(publicClient.RemoveAsync)).ConfigureAwait(false);
        }
    }

    private async Task<string?> TrySilentSignInCore()
    {
        if (publicClient is null) return null;
        try
        {
            var accounts = await publicClient.GetAccountsAsync();
            if (accounts.Any())
            {
                var result = await publicClient.AcquireTokenSilent(scopes, accounts.First()).ExecuteAsync().ConfigureAwait(false);
                return result.AccessToken;
            }
        }
        catch (MsalUiRequiredException)
        {
            // This exception is thrown when an interactive sign-in is required.
        }
        return null;
    }

    private async Task InitGraphClient()
    {
        if (publicClient is null) return;
        var authenticationProvider = new BaseBearerTokenAuthenticationProvider(new TokenProvider(publicClient));
        graphClient = new GraphServiceClient(authenticationProvider);            
        var user = await graphClient.Me.GetAsync();
        ArgumentNullException.ThrowIfNull(user);
        CurrentAccount = new(!string.IsNullOrEmpty(user.DisplayName) ? user.DisplayName : user.UserPrincipalName ?? "", user.Mail);
    }

    public async Task<(Stream? stream, string? cTag)> DownloadFile(string? cTag)
    {
        if (graphClient == null) return default;
        var item = await GetItem(dataFileName).ConfigureAwait(false);
        var metaItem = await item.GetAsync().ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(metaItem);
        if (metaItem.CTag != cTag)
        {
            var result = (await item.Content.GetAsync().ConfigureAwait(false), metaItem.CTag);
            Log.Default.Info("WebStorageService.DownloadFile completed. CTag: {0}", metaItem.CTag);
            return result;
        }
        else Log.Default.Info("WebStorageService.DownloadFile: Same CTag: {0}", cTag);
        // TODO: catch (ServiceException ex) when (ex.StatusCode == HttpStatusCode.NotFound) { }
        return default;
    }

    public async Task<string?> UploadFile(Stream source)
    {
        if (graphClient == null) return null;
        var item = await GetItem(dataFileName).ConfigureAwait(false);
        var newItem = await item.Content.PutAsync(source).ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(newItem);
        Log.Default.Info("WebStorageService.UploadFile completed. CTag: {0}", newItem.CTag);
        return newItem.CTag;
    }

    private async Task<CustomDriveItemItemRequestBuilder> GetItem(string fileName)
    {
        if (graphClient is null) throw new InvalidOperationException("graphClient is null");
        var driveItem = await graphClient.Me.Drive.GetAsync().ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(driveItem);
        var appRootFolder = await graphClient.Drives[driveItem.Id].Special["AppRoot"].GetAsync().ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(appRootFolder);
        return graphClient.Drives[driveItem.Id].Items[appRootFolder.Id].ItemWithPath(fileName);
    }

    static partial void GetApplicationId(ref string? applicationId);


    private sealed class TokenProvider(IPublicClientApplication publicClient) : IAccessTokenProvider
    {
        public AllowedHostsValidator AllowedHostsValidator { get; } = new();

        public async Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
        {
            var accounts = await publicClient.GetAccountsAsync().ConfigureAwait(false);
            if (!accounts.Any()) return "";
            var result = await publicClient.AcquireTokenSilent(scopes, accounts.First()).ExecuteAsync(cancellationToken).ConfigureAwait(false);
            return result.AccessToken;
        }
    }
}
