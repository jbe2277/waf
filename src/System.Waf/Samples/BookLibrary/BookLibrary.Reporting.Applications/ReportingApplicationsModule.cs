using Autofac;
using System.Waf.Applications;
using Waf.BookLibrary.Reporting.Applications.Controllers;
using Waf.BookLibrary.Reporting.Applications.ViewModels;

namespace Waf.BookLibrary.Reporting.Applications;

public sealed class ReportingApplicationsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ModuleController>().As<IModuleController>().AsSelf().SingleInstance();

        builder.RegisterType<ReportViewModel>().AsSelf().SingleInstance();
    }
}
