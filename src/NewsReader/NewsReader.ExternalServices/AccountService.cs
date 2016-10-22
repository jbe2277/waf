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
using Windows.UI.ApplicationSettings;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(IAccountService)), Export(typeof(IAccountInfoService)), Shared]
    internal class AccountService : Model, IAccountService
    {
        private const string tokenScope = "onedrive.appfolder";
        private WebAccountProvider provider;
        private WebAccount account;
        private UserAccount currentAccount;


        public UserAccount CurrentAccount
        {
            get { return currentAccount; }
            set { SetProperty(ref currentAccount, value); }
        }


        public void SignIn()
        {
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += BuildAccountsSettingsPaneAsync;
            AccountsSettingsPane.Show();
        }

        public async Task SignOutAsync()
        {
            await account.SignOutAsync();
            CurrentAccount = null;
        }

        private async void BuildAccountsSettingsPaneAsync(AccountsSettingsPane s, AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            s.AccountCommandsRequested -= BuildAccountsSettingsPaneAsync;

            var deferral = e.GetDeferral();

            e.HeaderText = "TODO: Explain why the user should sign in.";
            var msaProvider = await WebAuthenticationCoreManager.FindAccountProviderAsync("https://login.microsoft.com", "consumers");
            var command = new WebAccountProviderCommand(msaProvider, GetMsaTokenAsync);
            e.WebAccountProviderCommands.Add(command);

            deferral.Complete();
        }

        private async void GetMsaTokenAsync(WebAccountProviderCommand command)
        {
            var request = new WebTokenRequest(command.WebAccountProvider, tokenScope);
            var result = await WebAuthenticationCoreManager.RequestTokenAsync(request);

            if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                provider = command.WebAccountProvider;
                account = result.ResponseData[0].WebAccount;
                var userName = await GetUserNameAsync(result.ResponseData[0].Token);
                CurrentAccount = new UserAccount(userName);
            }
        }

        private async Task<string> GetUserNameAsync(string token)
        {
            var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + token);
            using (var client = new HttpClient())
            using (var result = await client.GetAsync(restApi))
            {
                string content = await result.Content.ReadAsStringAsync();

                var jsonObject = JsonObject.Parse(content);
                string name = jsonObject["name"].GetString();
                return name;
            }
        }
    }
}
