using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.Common.Applications;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications;

[TestClass]
public abstract class InfrastructureTest : ApplicationsTest
{
    protected override void ConfigureContainer(ContainerBuilder builder)
    {
        base.ConfigureContainer(builder);
        builder.RegisterModule(new InfrastructureApplicationsModule());
        builder.RegisterModule(new MockInfrastructurePresentationModule());
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        var environmentService = Get<MockSystemService>();
        environmentService.DataDirectory = Path.Combine(Environment.CurrentDirectory, "Data");
    }
}
