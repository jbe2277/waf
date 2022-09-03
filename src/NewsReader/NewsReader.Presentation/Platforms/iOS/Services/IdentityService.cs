using Foundation;
using Microsoft.Identity.Client;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.Presentation.Platforms.iOS.Services
{
    public class IdentityService : IIdentityService
    {
        public void Build(PublicClientApplicationBuilder builder)
        {
            builder.WithIosKeychainSecurityGroup(NSBundle.MainBundle.BundleIdentifier);
        }

        public void Build(AcquireTokenInteractiveParameterBuilder builder)
        {
        }
    }
}