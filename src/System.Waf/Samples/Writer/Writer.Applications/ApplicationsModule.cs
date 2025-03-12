using Autofac;
using System.Waf.Applications;
using Waf.Writer.Applications.Controllers;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Applications;

public class ApplicationsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FileController>().AsSelf().SingleInstance();
        builder.RegisterType<ModuleController>().As<IModuleController>().AsSelf().SingleInstance();
        builder.RegisterType<PrintController>().AsSelf().SingleInstance();
        builder.RegisterType<RichTextDocumentController>().AsSelf().SingleInstance();

        builder.RegisterType<FileService>().As<IFileService>().AsSelf().SingleInstance();
        builder.RegisterType<ShellService>().As<IShellService>().AsSelf().SingleInstance();

        builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<PrintPreviewViewModel>().AsSelf();
        builder.RegisterType<RichTextViewModel>().AsSelf();
        builder.RegisterType<SaveChangesViewModel>().AsSelf();
        builder.RegisterType<ShellViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<StartViewModel>().AsSelf().SingleInstance();
    }
}
