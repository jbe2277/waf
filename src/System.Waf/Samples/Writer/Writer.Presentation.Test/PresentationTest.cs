using Autofac;
using NLog;
using Test.Writer.Applications;
using Waf.Writer.Applications;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Presentation.Services;

namespace Test.Writer.Presentation;

public abstract class PresentationTest : ApplicationsTest
{
    protected PresentationTest()
    {
        LogManager.LogFactory.Dispose();  // Disable logging in unit tests
    }

    protected override void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new ApplicationsModule());
        builder.RegisterModule(new MockPresentationModule(useMockDocument: false));

        builder.RegisterType<RichTextDocumentType>().As<IRichTextDocumentType>().SingleInstance();
        builder.RegisterType<XpsExportDocumentType>().As<IXpsExportDocumentType>().SingleInstance();
    }
}
