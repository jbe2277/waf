using Autofac;
using System.Waf.Applications;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;

namespace Waf.BookLibrary.Library.Applications;

public class ApplicationsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BookController>().AsSelf().SingleInstance();
        builder.RegisterType<EntityController>().As<IEntityController>().AsSelf().SingleInstance();
        builder.RegisterType<ModuleController>().As<IModuleController>().AsSelf().SingleInstance();
        builder.RegisterType<PersonController>().AsSelf().SingleInstance();

        builder.RegisterType<EntityService>().As<IEntityService>().AsSelf().SingleInstance();
        builder.RegisterType<ShellService>().As<IShellService>().AsSelf().SingleInstance();

        builder.RegisterType<BookListViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<BookViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<LendToViewModel>().AsSelf();
        builder.RegisterType<PersonListViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<PersonViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<ShellViewModel>().AsSelf().SingleInstance();
    }
}
