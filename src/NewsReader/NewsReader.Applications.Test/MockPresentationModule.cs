using Autofac;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting.Mocks;
using Test.NewsReader.Applications.Services;
using Test.NewsReader.Applications.Views;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.Views;

namespace Test.NewsReader.Applications;

public class MockPresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockAppInfoService>().AsSelf().As<IAppInfoService>().SingleInstance();
        builder.RegisterType<MockDataService>().AsSelf().As<IDataService>().SingleInstance();
        builder.RegisterType<MockLauncherService>().AsSelf().As<ILauncherService>().SingleInstance();
        builder.RegisterType<MockMessageService>().AsSelf().As<IMessageService>().SingleInstance();
        builder.RegisterType<MockNetworkInfoService>().AsSelf().As<INetworkInfoService>().SingleInstance();
        builder.RegisterType<MockSettingsService>().AsSelf().As<ISettingsService>().SingleInstance();
        builder.RegisterType<MockSyndicationService>().AsSelf().As<ISyndicationService>().SingleInstance();
        builder.RegisterType<MockWebStorageService>().AsSelf().As<IWebStorageService>().SingleInstance();

        builder.RegisterType<MockFeedView>().AsSelf().As<IFeedView>().SingleInstance();
        builder.RegisterType<MockSettingsView>().AsSelf().As<ISettingsView>().SingleInstance();
        builder.RegisterType<MockShellView>().AsSelf().As<IShellView>().SingleInstance();
    }
}

