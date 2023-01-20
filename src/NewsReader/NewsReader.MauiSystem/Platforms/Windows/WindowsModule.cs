using Autofac;
using Waf.NewsReader.MauiSystem.Platforms.Windows.Services;
using Waf.NewsReader.Presentation.Services;

namespace Waf.NewsReader.MauiSystem.Platforms.Windows;

internal sealed class WindowsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<WindowsSystemTraceListener>().As<SystemTraceListener>().SingleInstance();
    }
}
