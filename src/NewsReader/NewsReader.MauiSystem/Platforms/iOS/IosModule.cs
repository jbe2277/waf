using Autofac;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.MauiSystem.Platforms.iOS.Services;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.MauiSystem.Platforms.iOS;

internal sealed class IosModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<IdentityService>().As<IIdentityService>().SingleInstance();
        builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();
    }
}
