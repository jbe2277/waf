using Autofac;
using Test.InformationManager.EmailClient.Modules.Applications.Services;
using Test.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Interfaces.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications;

public sealed class MockEmailClientPresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockAddressBookService>().As<IAddressBookService>().AsSelf().SingleInstance();

        builder.RegisterType<MockBasicEmailAccountView>().As<IBasicEmailAccountView>();
        builder.RegisterType<MockEditEmailAccountView>().As<IEditEmailAccountView>();
        builder.RegisterType<MockEmailAccountsView>().As<IEmailAccountsView>();
        builder.RegisterType<MockEmailLayoutView>().As<IEmailLayoutView>().SingleInstance();
        builder.RegisterType<MockEmailListView>().As<IEmailListView>();
        builder.RegisterType<MockEmailView>().As<IEmailView>();
        builder.RegisterType<MockExchangeSettingsView>().As<IExchangeSettingsView>();
        builder.RegisterType<MockNewEmailView>().As<INewEmailView>();
        builder.RegisterType<MockPop3SettingsView>().As<IPop3SettingsView>();
    }
}