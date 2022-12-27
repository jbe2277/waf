using Microsoft.Identity.Client;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.MauiSystem.Platforms.Android.Services;

public class IdentityService : IIdentityService
{
    public void Build(PublicClientApplicationBuilder builder)
    {
        builder.WithParentActivityOrWindow(() => Platform.CurrentActivity);
    }
}