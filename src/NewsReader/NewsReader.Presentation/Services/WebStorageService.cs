using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services;

// Microsoft Azure / Microsoft Entry ID / App registration
// - Redirect URI: wafe5b8cee6-8ba0-46c5-96ef-a3c8a1e2bb26://auth
//
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
#elif WINDOWS
            Microsoft.Identity.Client.Broker.BrokerExtension.WithBroker(builder, new BrokerOptions(BrokerOptions.OperatingSystems.Windows)
            {
                Title = AppInfo.Name
            });
            builder.WithParentActivityOrWindow(() => WinRT.Interop.WindowNative.GetWindowHandle(App.CurrentWindow!.Handler.PlatformView!));
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
            var storageProperties = new Microsoft.Identity.Client.Extensions.Msal.StorageCreationPropertiesBuilder("msal.dat", FileSystem.CacheDirectory).Build();
            var cacheHelper = await Microsoft.Identity.Client.Extensions.Msal.MsalCacheHelper.CreateAsync(storageProperties);
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
        Log.Default.Trace("WebStorageService.DownloadFile started");
        var itemRequest = await GetItemRequest(dataFileName).ConfigureAwait(false);
        DriveItem? item;
        try
        {
            item = await itemRequest.GetAsync().ConfigureAwait(false);
        }
        catch (ODataError odataError) when (odataError.Error?.Code?.Equals(GraphErrorCode.ItemNotFound.ToString(), StringComparison.OrdinalIgnoreCase) == true)
        {
            Log.Default.Info("WebStorageService.DownloadFile: Item not found");
            return default;
        }
        ArgumentNullException.ThrowIfNull(item);
        if (item.CTag != cTag)
        {
            var result = (await itemRequest.Content.GetAsync().ConfigureAwait(false), item.CTag);
            Log.Default.Info("WebStorageService.DownloadFile completed. CTag: {0}", item.CTag);
            return result;
        }
        else Log.Default.Info("WebStorageService.DownloadFile: Same CTag: {0}", cTag);
        return default;
    }

    public async Task<string?> UploadFile(Stream source)
    {
        if (graphClient == null) return null;
        Log.Default.Trace("WebStorageService.UploadFile started");
        var itemRequest = await GetItemRequest(dataFileName).ConfigureAwait(false);
        var newItem = await itemRequest.Content.PutAsync(source).ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(newItem);
        Log.Default.Info("WebStorageService.UploadFile completed. CTag: {0}", newItem.CTag);
        return newItem.CTag;
    }

    private async Task<CustomDriveItemItemRequestBuilder> GetItemRequest(string fileName)
    {
        if (graphClient is null) throw new InvalidOperationException("graphClient is null");

        // Using workaround to get the driveId because AppFolder scope does not allow to the read Drive directly. https://github.com/microsoftgraph/msgraph-sdk-dotnet/issues/2624
        var result = await graphClient.Me.Drive.WithUrl($"{graphClient.RequestAdapter.BaseUrl}/drive/special/approot:/{fileName}").GetAsync().ConfigureAwait(false);
        var driveId = result?.Id?.Split("!")[0] ?? throw new InvalidOperationException("graphClient: not able to get the driveId");
        
        var appRootFolder = await graphClient.Drives[driveId].Special["AppRoot"].GetAsync().ConfigureAwait(false)
                ?? throw new InvalidOperationException("graphClient: not able to get the appRootFolder");
        return graphClient.Drives[driveId].Items[appRootFolder.Id].ItemWithPath(fileName);
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
