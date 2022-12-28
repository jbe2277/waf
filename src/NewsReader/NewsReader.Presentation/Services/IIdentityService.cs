using Microsoft.Identity.Client;

namespace Waf.NewsReader.Presentation.Services;

public interface IIdentityService
{
    void Build(PublicClientApplicationBuilder builder);
}
