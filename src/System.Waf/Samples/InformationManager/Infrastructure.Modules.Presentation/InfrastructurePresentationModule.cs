using Autofac;
using Waf.InformationManager.Common.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Applications.Views;
using Waf.InformationManager.Infrastructure.Modules.Presentation.Services;
using Waf.InformationManager.Infrastructure.Modules.Presentation.Views;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation;

public sealed class InfrastructurePresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PresentationService>().As<IPresentationService>().SingleInstance();
        builder.RegisterType<SystemService>().As<ISystemService>().SingleInstance();

        builder.RegisterType<ShellWindow>().As<IShellView>().SingleInstance();
    }
}
