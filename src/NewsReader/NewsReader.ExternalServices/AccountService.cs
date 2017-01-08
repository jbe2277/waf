using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Domain;
using System;
using System.Composition;
using System.Net.Http;
using System.Threading.Tasks;
using System.Waf.Foundation;
using Windows.Data.Json;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.ApplicationSettings;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(IAccountService)), Export(typeof(IAccountInfoService)), Shared]
    internal class AccountService : Model, IAccountService
    {
        private const string tokenScope = "onedrive.appfolder";
        private readonly IResourceService resourceService;
        private WebAccount webAccount;
        private UserAccount currentAccount;
        private Action<Task<UserAccount>> signInStartedCallback;


        [ImportingConstructor]
        public AccountService(IResourceService resourceService)
        {
            this.resourceService = resourceService;
        }


        public UserAccount CurrentAccount
        {
            get { return currentAccount; }
            set { SetProperty(ref currentAccount, value); }
        }


        public async Task InitializeAsync()
        {
            var token = await GetTokenAsync();
            if (token != null)
            {
                CurrentAccount = await CreateUserAccount(token);
            }
        }

        public void SignIn(Action<Task<UserAccount>> signInStarted)
        {
            signInStartedCallback = signInStarted;
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += BuildAccountsSettingsPaneAsync;
            AccountsSettingsPane.Show();
        }

        public async Task SignOutAsync()
        {
            await webAccount?.SignOutAsync();
            UpdateWebAccount(null);
            CurrentAccount = null;
        }

        public async Task<string> GetTokenAsync()
        {
            var webAccount = await GetWebAccount().ConfigureAwait(false);
            if (webAccount == null)
            {
                return null;
            }

            var request = new WebTokenRequest(webAccount.WebAccountProvider, tokenScope);
            var result = await WebAuthenticationCoreManager.GetTokenSilentlyAsync(request, webAccount).AsTask().ConfigureAwait(false);

            if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                return result.ResponseData[0].Token;
            }
            return null;
        }

        private async void BuildAccountsSettingsPaneAsync(AccountsSettingsPane s, AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            s.AccountCommandsRequested -= BuildAccountsSettingsPaneAsync;

            var deferral = e.GetDeferral();

            e.HeaderText = resourceService.GetString("SignInDescription");
            var msaProvider = await WebAuthenticationCoreManager.FindAccountProviderAsync("https://login.microsoft.com", "consumers");
            var command = new WebAccountProviderCommand(msaProvider, GetMsaToken);
            e.WebAccountProviderCommands.Add(command);

            deferral.Complete();
        }

        private void GetMsaToken(WebAccountProviderCommand command)
        {
            CurrentAccount = null;
            var task = GetMsaTokenAsync(command);
            signInStartedCallback(task);
        }

        private async Task<UserAccount> GetMsaTokenAsync(WebAccountProviderCommand command)
        {
            var request = new WebTokenRequest(command.WebAccountProvider, tokenScope);
            var result = await WebAuthenticationCoreManager.RequestTokenAsync(request);

            if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                UpdateWebAccount(result.ResponseData[0].WebAccount);
                CurrentAccount = await CreateUserAccount(result.ResponseData[0].Token);
                return CurrentAccount;
            }
            else
            {
                throw new InvalidOperationException("WebAuthentication Response: " + result.ResponseStatus);
            }
        }

        private async Task<WebAccount> GetWebAccount()
        {
            if (webAccount != null)
            {
                return webAccount;
            }

            string providerId = ApplicationData.Current.LocalSettings.Values["CurrentUserProviderId"]?.ToString();
            string accountId = ApplicationData.Current.LocalSettings.Values["CurrentUserId"]?.ToString();

            if (null == providerId || null == accountId)
            {
                return null;
            }

            var provider = await WebAuthenticationCoreManager.FindAccountProviderAsync(providerId).AsTask().ConfigureAwait(false);
            webAccount = await WebAuthenticationCoreManager.FindAccountAsync(provider, accountId).AsTask().ConfigureAwait(false);
            return webAccount;
        }

        private void UpdateWebAccount(WebAccount newAccount)
        {
            webAccount = newAccount;
            if (webAccount != null)
            {
                ApplicationData.Current.LocalSettings.Values["CurrentUserProviderId"] = webAccount.WebAccountProvider.Id;
                ApplicationData.Current.LocalSettings.Values["CurrentUserId"] = webAccount.Id;
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values.Remove("CurrentUserProviderId");
                ApplicationData.Current.LocalSettings.Values.Remove("CurrentUserId");
            }            
        }

        private async Task<UserAccount> CreateUserAccount(string token)
        {
            string userName = webAccount.UserName;
            try
            {
                var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + token);
                using (var client = new HttpClient())
                using (var result = await client.GetAsync(restApi).ConfigureAwait(false))
                {
                    string content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var jsonObject = JsonObject.Parse(content);
                    userName = jsonObject["name"].GetString();
                }
            }
            catch (Exception)
            {
                // Do not care if we cannot get the user name from Microsoft's web api.
            }
            return new UserAccount(webAccount.Id, userName);
        }
    }
}
