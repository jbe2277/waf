using Autofac;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using Test.InformationManager.Infrastructure.Modules.Applications.Views;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Applications.Views;

namespace Test.InformationManager.Infrastructure.Modules.Applications;

public sealed class MockInfrastructurePresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockSystemService>().As<ISystemService>().AsSelf().SingleInstance();

        builder.RegisterType<MockShellView>().As<IShellView>().AsSelf().SingleInstance();
    }
}
