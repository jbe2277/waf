using Microsoft.Identity.Client;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.Presentation.Platforms.Android.Services
{
    public class IdentityService : IIdentityService
    {
        public void Build(PublicClientApplicationBuilder builder)
        {
        }

        public void Build(AcquireTokenInteractiveParameterBuilder builder)
        {
            builder.WithParentActivityOrWindow(MainActivity.Current);
        }
    }
}