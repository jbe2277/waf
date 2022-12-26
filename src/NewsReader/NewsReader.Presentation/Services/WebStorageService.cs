using Microsoft.AppCenter.Crashes;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Waf.Foundation;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services
{
    // Add App.xaml.key.cs file with:
    //partial class WebStorageService
    //{
    //    static partial void GetApplicationId(ref string applicationId)
    //    {
    //        applicationId = "{key}";
    //    }
    //}

    internal partial class WebStorageService : Model, IWebStorageService
    {
        private const string dataFileName = "data.zip";
        private static readonly string[] scopes = { "User.Read", "Files.ReadWrite.AppFolder" };

        private readonly IPublicClientApplication? publicClient;
        private GraphServiceClient? graphClient;
        private UserAccount? currentAccount;

        public WebStorageService(IIdentityService? identityService = null)
        {
            string? appId = null;
            GetApplicationId(ref appId);
            if (appId != null)
            {
                var builder = PublicClientApplicationBuilder.Create(appId);
                builder.WithRedirectUri("wafe5b8cee6-8ba0-46c5-96ef-a3c8a1e2bb26://auth");
                identityService?.Build(builder);
                publicClient = builder.Build();
            }
        }

        public UserAccount? CurrentAccount
        {
            get => currentAccount;
            private set => SetProperty(ref currentAccount, value);
        }

        public async Task<bool> TrySilentSignIn()
        {
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
                Log.Default.Warn("Silent login failed: {0}", ex);
                Crashes.TrackError(ex);
                // Ignore (e.g. no internet access)
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
                    var interactiveRequest = publicClient.AcquireTokenInteractive(scopes);
                    await interactiveRequest.ExecuteAsync();
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
            if (publicClient == null) return null;
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
            graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                async requestMessage =>
                {
                    if (publicClient == null) return;
                    var accounts = await publicClient.GetAccountsAsync().ConfigureAwait(false);
                    if (!accounts.Any()) return;
                    var result = await publicClient.AcquireTokenSilent(scopes, accounts.First()).ExecuteAsync().ConfigureAwait(false);
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                }));
            var user = await graphClient.Me.Request().GetAsync();
            CurrentAccount = new UserAccount(!string.IsNullOrEmpty(user.DisplayName) ? user.DisplayName : user.UserPrincipalName, user.Mail);
        }

        public async Task<(Stream? stream, string? cTag)> DownloadFile(string? cTag)
        {
            if (graphClient == null) return default;
            var item = graphClient.Me.Drive.Special.AppRoot.ItemWithPath(dataFileName);
            try
            {
                var metaItem = await item.Request().GetAsync().ConfigureAwait(false);
                if (metaItem.CTag != cTag)
                {
                    return (await item.Content.Request().GetAsync().ConfigureAwait(false), metaItem.CTag);
                }
            }
            catch (ServiceException ex) when (ex.StatusCode == HttpStatusCode.NotFound) { }
            return default;
        }

        public async Task<string?> UploadFile(Stream source)
        {
            if (graphClient == null) return null;
            var driveItem = await graphClient.Me.Drive.Special.AppRoot.ItemWithPath(dataFileName)
                .Content.Request().PutAsync<DriveItem>(source).ConfigureAwait(false);
            return driveItem.CTag;
        }

        static partial void GetApplicationId(ref string? applicationId);
    }
}
