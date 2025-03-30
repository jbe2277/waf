using Autofac;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Presentation.Views;

namespace Waf.InformationManager.EmailClient.Modules.Presentation;

public sealed class EmailClientPresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BasicEmailAccountView>().As<IBasicEmailAccountView>();
        builder.RegisterType<EditEmailAccountWindow>().As<IEditEmailAccountView>();
        builder.RegisterType<EmailAccountsWindow>().As<IEmailAccountsView>();
        builder.RegisterType<EmailLayoutView>().As<IEmailLayoutView>().SingleInstance();
        builder.RegisterType<EmailListView>().As<IEmailListView>();
        builder.RegisterType<EmailView>().As<IEmailView>();
        builder.RegisterType<ExchangeSettingsView>().As<IExchangeSettingsView>();
        builder.RegisterType<NewEmailWindow>().As<INewEmailView>();
        builder.RegisterType<Pop3SettingsView>().As<IPop3SettingsView>();
    }
}