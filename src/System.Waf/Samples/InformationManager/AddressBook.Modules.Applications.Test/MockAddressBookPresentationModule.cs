using Autofac;
using Test.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Test.InformationManager.AddressBook.Modules.Applications;

public sealed class MockAddressBookPresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockContactLayoutView>().As<IContactLayoutView>().SingleInstance();
        builder.RegisterType<MockContactListView>().As<IContactListView>();
        builder.RegisterType<MockContactView>().As<IContactView>();
        builder.RegisterType<MockSelectContactView>().As<ISelectContactView>();
    }
}
