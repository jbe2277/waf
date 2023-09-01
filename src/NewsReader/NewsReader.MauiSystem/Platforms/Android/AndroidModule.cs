using Autofac;
using Waf.NewsReader.MauiSystem.Platforms.Android.Services;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.MauiSystem.Platforms.Android;

internal sealed class AndroidModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<IdentityService>().As<IIdentityService>().SingleInstance();
    }
}
