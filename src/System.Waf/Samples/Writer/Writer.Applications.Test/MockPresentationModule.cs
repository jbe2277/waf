using Autofac;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting.Mocks;
using Test.Writer.Applications.Documents;
using Test.Writer.Applications.Services;
using Test.Writer.Applications.Views;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications;

public class MockPresentationModule(bool useMockDocument = true) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockFileDialogService>().As<IFileDialogService>().AsSelf().SingleInstance();
        builder.RegisterType<MockMessageService>().As<IMessageService>().AsSelf().SingleInstance();
        builder.RegisterType<MockSettingsService>().As<ISettingsService>().AsSelf().SingleInstance();
        
        builder.RegisterType<MockPrintDialogService>().As<IPrintDialogService>().AsSelf().SingleInstance();
        builder.RegisterType<MockSystemService>().As<ISystemService>().AsSelf().SingleInstance();
        if (useMockDocument)
        {
            builder.RegisterType<MockRichTextDocumentType>().As<IRichTextDocumentType>().AsSelf().SingleInstance();
            builder.RegisterType<MockXpsExportDocumentType>().As<IXpsExportDocumentType>().AsSelf().SingleInstance();
        }

        builder.RegisterType<MockMainView>().As<IMainView>().AsSelf().SingleInstance();
        builder.RegisterType<MockPrintPreviewView>().As<IPrintPreviewView>();
        builder.RegisterType<MockRichTextView>().As<IRichTextView>();
        builder.RegisterType<MockSaveChangesView>().As<ISaveChangesView>();
        builder.RegisterType<MockShellView>().As<IShellView>().AsSelf().SingleInstance();
        builder.RegisterType<MockStartView>().As<IStartView>().AsSelf().SingleInstance();
    }
}

