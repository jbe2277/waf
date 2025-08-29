using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Security.Authentication;

namespace Waf.NewsReader.Presentation.Services;

internal sealed class MsalAuthorizationHandler : DelegatingHandler
{
    private readonly IPublicClientApplication client;
    private readonly string[] scopes;

    public MsalAuthorizationHandler(IPublicClientApplication client, string[] scopes)
    {
        this.client = client;
        this.scopes = scopes;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accounts = await client.GetAccountsAsync().ConfigureAwait(false);
        if (!accounts.Any()) throw new AuthenticationException("No account found");
        var result = await client.AcquireTokenSilent(scopes, accounts.First()).ExecuteAsync(cancellationToken).ConfigureAwait(false);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
