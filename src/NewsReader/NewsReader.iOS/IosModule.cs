using Autofac;
using Waf.NewsReader.iOS.Services;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.iOS
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdentityService>().As<IIdentityService>().SingleInstance();
        }
    }
}