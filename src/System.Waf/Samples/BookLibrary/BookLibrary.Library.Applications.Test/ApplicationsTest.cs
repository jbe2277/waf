using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Library.Applications;
using IContainer = Autofac.IContainer;

namespace Test.BookLibrary.Library.Applications;

[TestClass]
public abstract class ApplicationsTest
{
    public IContainer Container { get; private set; } = null!;

    [TestInitialize]
    public void Initialize()
    {
        var builder = new ContainerBuilder();
        ConfigureContainer(builder);
        Container = builder.Build();

        OnInitialize();
    }

    [TestCleanup]
    public void Cleanup()
    {
        OnCleanup();
        Container?.Dispose();
    }

    public T Get<T>() where T : notnull => Container.Resolve<T>();

    public Lazy<T> GetLazy<T>() where T : notnull => new(Get<T>);

    protected virtual void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new ApplicationsModule());
        builder.RegisterModule(new MockPresentationModule());
    }

    protected virtual void OnInitialize() { }

    protected virtual void OnCleanup() { }
}
