using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.Common.Applications;
using Test.InformationManager.Infrastructure.Modules.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications;

namespace Test.InformationManager.EmailClient.Modules.Applications;

[TestClass]
public abstract class EmailClientTest : ApplicationsTest
{
    protected override void ConfigureContainer(ContainerBuilder builder)
    {
        base.ConfigureContainer(builder);
        builder.RegisterModule(new InfrastructureApplicationsModule());
        builder.RegisterModule(new MockInfrastructurePresentationModule());

        builder.RegisterModule(new EmailClientApplicationsModule());
        builder.RegisterModule(new MockEmailClientPresentationModule());
    }
}
