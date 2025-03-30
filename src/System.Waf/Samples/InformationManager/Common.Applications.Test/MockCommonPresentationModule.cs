using Autofac;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting.Mocks;

namespace Test.InformationManager.Common.Applications;

public sealed class MockCommonPresentationModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockMessageService>().As<IMessageService>().AsSelf().SingleInstance();
        builder.RegisterType<MockSettingsService>().As<ISettingsService>().AsSelf().SingleInstance();
    }
}