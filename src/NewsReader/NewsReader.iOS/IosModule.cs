using Autofac;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.iOS.Services;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.iOS
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdentityService>().As<IIdentityService>().SingleInstance();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();
        }
    }
}