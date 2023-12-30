using Autofac;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.MauiSystem.Platforms.iOS.Services;

namespace Waf.NewsReader.MauiSystem.Platforms.iOS;

internal sealed class IosModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();
    }
}
