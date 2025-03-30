using Autofac;
using System.Waf.Applications;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Controllers;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Applications.ViewModels;

namespace Waf.InformationManager.Infrastructure.Modules.Applications;

public sealed class InfrastructureApplicationsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DocumentController>().As<IDocumentService>().AsSelf().SingleInstance();
        builder.RegisterType<ModuleController>().As<IModuleController>().AsSelf().SingleInstance();

        builder.RegisterType<NavigationService>().As<INavigationService>().AsSelf().SingleInstance();
        builder.RegisterType<ShellService>().As<IShellService>().AsSelf().SingleInstance();

        builder.RegisterType<ShellViewModel>().As<IShellViewModel>().AsSelf().SingleInstance();
    }
}