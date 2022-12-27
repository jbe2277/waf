using Autofac;
using Waf.NewsReader.Presentation;

namespace Waf.NewsReader.MauiSystem;

public class MauiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<App>().SingleInstance();
    }
}