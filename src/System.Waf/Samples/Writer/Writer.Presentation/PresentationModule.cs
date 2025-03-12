using Autofac;
using System.Waf.Applications.Services;
using System.Waf.Presentation.Services;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;
using Waf.Writer.Presentation.Services;
using Waf.Writer.Presentation.Views;

namespace Waf.Writer.Presentation;

public class PresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FileDialogService>().As<IFileDialogService>().AsSelf().SingleInstance();
        builder.RegisterType<MessageService>().As<IMessageService>().SingleInstance();
        builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();

        builder.RegisterType<PrintDialogService>().As<IPrintDialogService>().SingleInstance();
        builder.RegisterType<RichTextDocumentType>().As<IRichTextDocumentType>().SingleInstance();
        builder.RegisterType<SystemService>().As<ISystemService>().SingleInstance();
        builder.RegisterType<XpsExportDocumentType>().As<IXpsExportDocumentType>().SingleInstance();

        builder.RegisterType<MainView>().As<IMainView>().SingleInstance();
        builder.RegisterType<PrintPreviewView>().As<IPrintPreviewView>();
        builder.RegisterType<RichTextView>().As<IRichTextView>();
        builder.RegisterType<SaveChangesWindow>().As<ISaveChangesView>();
        builder.RegisterType<ShellWindow>().As<IShellView>().SingleInstance();
        builder.RegisterType<StartView>().As<IStartView>().SingleInstance();
    }
}
