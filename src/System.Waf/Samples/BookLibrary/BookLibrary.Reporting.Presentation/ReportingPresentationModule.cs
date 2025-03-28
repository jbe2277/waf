using Autofac;
using Waf.BookLibrary.Reporting.Applications.Reports;
using Waf.BookLibrary.Reporting.Applications.Views;
using Waf.BookLibrary.Reporting.Presentation.Reports;
using Waf.BookLibrary.Reporting.Presentation.Views;

namespace Waf.BookLibrary.Reporting.Presentation;

public sealed class ReportingPresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BookListReport>().As<IBookListReport>();
        builder.RegisterType<BorrowedBooksReport>().As<IBorrowedBooksReport>();

        builder.RegisterType<ReportView>().As<IReportView>().SingleInstance();
    }
}
