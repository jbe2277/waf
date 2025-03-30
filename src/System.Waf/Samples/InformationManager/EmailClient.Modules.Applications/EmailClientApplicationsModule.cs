using Autofac;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Controllers;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

namespace Waf.InformationManager.EmailClient.Modules.Applications;

public sealed class EmailClientApplicationsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EditEmailAccountController>().AsSelf();
        builder.RegisterType<EmailAccountsController>().AsSelf().SingleInstance();
        builder.RegisterType<EmailFolderController>().AsSelf();
        builder.RegisterType<ModuleController>().As<IModuleController>().AsSelf().SingleInstance();
        builder.RegisterType<NewEmailController>().AsSelf();

        builder.RegisterType<BasicEmailAccountViewModel>().AsSelf();
        builder.RegisterType<EditEmailAccountViewModel>().AsSelf();
        builder.RegisterType<EmailAccountsViewModel>().AsSelf();
        builder.RegisterType<EmailLayoutViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<EmailListViewModel>().AsSelf();
        builder.RegisterType<EmailViewModel>().AsSelf();
        builder.RegisterType<ExchangeSettingsViewModel>().AsSelf();
        builder.RegisterType<NewEmailViewModel>().AsSelf();
        builder.RegisterType<Pop3SettingsViewModel>().AsSelf();
    }
}