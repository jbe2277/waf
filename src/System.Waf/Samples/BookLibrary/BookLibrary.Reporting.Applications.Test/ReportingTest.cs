using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.BookLibrary.Library.Applications;
using Waf.BookLibrary.Reporting.Applications;

namespace Test.BookLibrary.Reporting.Applications;

[TestClass]
public class ReportingTest : ApplicationsTest
{
    protected override void ConfigureContainer(ContainerBuilder builder)
    {
        base.ConfigureContainer(builder);
        builder.RegisterModule(new ReportingApplicationsModule());
        builder.RegisterModule(new MockReportingPresentationModule());
    }
}
