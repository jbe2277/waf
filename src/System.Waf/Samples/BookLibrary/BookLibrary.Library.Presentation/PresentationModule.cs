using Autofac;
using System.Waf.Applications.Services;
using System.Waf.Presentation.Services;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Presentation.Services;
using Waf.BookLibrary.Library.Presentation.Views;

namespace Waf.BookLibrary.Library.Presentation;

public class PresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MessageService>().As<IMessageService>().SingleInstance();
        builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();

        builder.RegisterType<DBContextService>().As<IDBContextService>().SingleInstance();
        builder.RegisterType<EmailService>().As<IEmailService>().SingleInstance();

        builder.RegisterType<BookListView>().As<IBookListView>().SingleInstance();
        builder.RegisterType<BookView>().As<IBookView>().SingleInstance();
        builder.RegisterType<LendToWindow>().As<ILendToView>();
        builder.RegisterType<PersonListView>().As<IPersonListView>().SingleInstance();
        builder.RegisterType<PersonView>().As<IPersonView>().SingleInstance();
        builder.RegisterType<ShellWindow>().As<IShellView>().SingleInstance();
    }
}
