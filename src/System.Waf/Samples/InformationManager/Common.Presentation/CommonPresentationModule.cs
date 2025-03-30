using Autofac;
using System.Waf.Applications.Services;
using System.Waf.Presentation.Services;

namespace Waf.InformationManager.Common.Presentation;

/// <summary>Autofac Module to register common services.</summary>
public sealed class CommonPresentationModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MessageService>().As<IMessageService>().SingleInstance();
        builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();
    }
}