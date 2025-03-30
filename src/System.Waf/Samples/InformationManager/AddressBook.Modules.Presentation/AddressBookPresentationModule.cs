using Autofac;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Presentation.Views;

namespace Waf.InformationManager.AddressBook.Modules.Presentation;

public sealed class AddressBookPresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ContactLayoutView>().As<IContactLayoutView>().SingleInstance();
        builder.RegisterType<ContactListView>().As<IContactListView>();
        builder.RegisterType<ContactView>().As<IContactView>();
        builder.RegisterType<SelectContactWindow>().As<ISelectContactView>();
    }
}