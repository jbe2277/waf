using Autofac;
using Waf.NewsReader.Android.Services;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.Android
{
    public class AndroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdentityService>().As<IIdentityService>().SingleInstance();
        }
    }
}