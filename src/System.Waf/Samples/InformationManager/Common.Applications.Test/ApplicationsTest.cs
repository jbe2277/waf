using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.Common.Domain;
using IContainer = Autofac.IContainer;

namespace Test.InformationManager.Common.Applications;

[TestClass]
public abstract class ApplicationsTest : DomainTest
{
    public IContainer Container { get; private set; } = null!;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        var builder = new ContainerBuilder();
        ConfigureContainer(builder);
        Container = builder.Build();
    }

    protected override void OnCleanup()
    {
        Container?.Dispose();
        base.OnCleanup();
    }

    public T Get<T>() where T : notnull => Container.Resolve<T>();

    public Lazy<T> GetLazy<T>() where T : notnull => new(Get<T>);

    protected virtual void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new MockCommonPresentationModule());
    }
}
