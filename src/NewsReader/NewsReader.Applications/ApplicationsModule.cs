using Autofac;
using Waf.NewsReader.Applications.Controllers;
using Waf.NewsReader.Applications.ViewModels;

namespace Waf.NewsReader.Applications
{
    public class ApplicationsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppController>().As<IAppController>().AsSelf().SingleInstance();

            builder.RegisterType<ShellViewModel>().SingleInstance();
        }
    }
}
