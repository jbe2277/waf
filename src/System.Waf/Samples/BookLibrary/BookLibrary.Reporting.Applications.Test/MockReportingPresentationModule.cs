using Autofac;
using Test.BookLibrary.Reporting.Applications.Reports;
using Test.BookLibrary.Reporting.Applications.Views;
using Waf.BookLibrary.Reporting.Applications.Reports;
using Waf.BookLibrary.Reporting.Applications.Views;

namespace Test.BookLibrary.Reporting.Applications;

public class MockReportingPresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockBookListReport>().As<IBookListReport>();
        builder.RegisterType<MockBorrowedBooksReport>().As<IBorrowedBooksReport>();

        builder.RegisterType<MockReportView>().As<IReportView>().SingleInstance();
    }
}
