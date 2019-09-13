using Autofac;
using Waf.NewsReader.Applications.Controllers;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.ViewModels;

namespace Waf.NewsReader.Applications
{
    public class ApplicationsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppController>().As<IAppController>().AsSelf().SingleInstance();
            builder.RegisterType<SettingsController>().SingleInstance();

            builder.RegisterType<FeedItemViewModel>().SingleInstance();
            builder.RegisterType<FeedViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();
            builder.RegisterType<ShellViewModel>().As<INavigationService>().AsSelf().SingleInstance();
        }
    }
}
