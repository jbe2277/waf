using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Waf.Writer.Applications;
using Waf.Writer.Applications.Controllers;
using IContainer = Autofac.IContainer;

namespace Test.Writer.Applications;

public abstract class ApplicationsTest
{
    private ModuleController? moduleController;

    protected IContainer Container { get; private set; } = null!;

    [TestInitialize]
    public void Initialize()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var builder = new ContainerBuilder();
        OnSetupBuilder(builder);
        Container = builder.Build();

        OnInitialize();
    }

    [TestCleanup]
    public void Cleanup()
    {
        Container?.Dispose();
        OnCleanup();
    }

    public T Get<T>() where T : notnull => Container.Resolve<T>();

    public Lazy<T> GetLazy<T>() where T : notnull => new(Get<T>);

    protected virtual void OnSetupBuilder(ContainerBuilder builder)
    {
        builder.RegisterModule(new ApplicationsModule());
        builder.RegisterModule(new MockPresentationModule());
    }

    protected virtual void OnInitialize() { }

    protected virtual void OnCleanup() 
    {
        moduleController?.Shutdown();
    }

    protected void StartApp()
    {
        moduleController = Get<ModuleController>();
        moduleController.Initialize();
        moduleController.Run();
    }
}
