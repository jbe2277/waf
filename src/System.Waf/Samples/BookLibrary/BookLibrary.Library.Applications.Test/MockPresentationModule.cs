using Autofac;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting.Mocks;
using Test.BookLibrary.Library.Applications.Controllers;
using Test.BookLibrary.Library.Applications.Services;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.Views;

namespace Test.BookLibrary.Library.Applications;

public class MockPresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockMessageService>().As<IMessageService>().AsSelf().SingleInstance();
        builder.RegisterType<MockSettingsService>().As<ISettingsService>().AsSelf().SingleInstance();

        builder.RegisterType<MockEntityController>().AsSelf().AsSelf().SingleInstance();

        builder.RegisterType<MockDBContextService>().As<IDBContextService>().AsSelf().SingleInstance();
        builder.RegisterType<MockEmailService>().As<IEmailService>().AsSelf().SingleInstance();

        builder.RegisterType<MockBookListView>().As<IBookListView>().AsSelf().SingleInstance();
        builder.RegisterType<MockBookView>().As<IBookView>().AsSelf().SingleInstance();
        builder.RegisterType<MockLendToView>().As<ILendToView>();
        builder.RegisterType<MockPersonListView>().As<IPersonListView>().AsSelf().SingleInstance();
        builder.RegisterType<MockPersonView>().As<IPersonView>().AsSelf().SingleInstance();
        builder.RegisterType<MockShellView>().As<IShellView>().AsSelf().SingleInstance();
    }
}
