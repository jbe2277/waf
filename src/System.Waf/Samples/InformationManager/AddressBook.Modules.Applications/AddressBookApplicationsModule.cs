using Autofac;
using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Interfaces.Applications;
using Waf.InformationManager.AddressBook.Modules.Applications.Controllers;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

namespace Waf.InformationManager.AddressBook.Modules.Applications;

public sealed class AddressBookApplicationsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ContactController>().AsSelf();
        builder.RegisterType<ModuleController>().As<IModuleController>().As<IAddressBookService>().AsSelf().SingleInstance();
        builder.RegisterType<SelectContactController>().AsSelf();

        builder.RegisterType<ContactLayoutViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<ContactListViewModel>().AsSelf();
        builder.RegisterType<ContactViewModel>().AsSelf();
        builder.RegisterType<SelectContactViewModel>().AsSelf();
    }
}