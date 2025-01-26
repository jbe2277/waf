using Autofac;
using System.Waf.Applications;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Applications.Controllers;
using Test.NewsReader.Applications.Views;
using System.Waf.UnitTesting;

namespace Test.NewsReader.Applications;

public record ViewPair<TView, TViewModel>(TView View) where TView : IView where TViewModel : IViewModelCore
{
    public TViewModel ViewModel => (TViewModel)View.DataContext!;
}

public abstract class TestBase : IDisposable
{
    protected TestBase()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule(new ApplicationsModule());
        builder.RegisterModule(new MockPresentationModule());
        Container = builder.Build();
        Context = UnitTestSynchronizationContext.Create();
    }

    public Autofac.IContainer Container { get; }

    public UnitTestSynchronizationContext Context { get; }

    internal AppController AppController { get; private set; } = null!;
    internal FeedsController FeedsController { get; private set; } = null!;
    internal SettingsController SettingsController { get; private set; } = null!;
    public ViewPair<MockShellView, ShellViewModel> Shell { get; private set; } = null!;

    public T Resolve<T>() where T : notnull => Container.Resolve<T>();

    public void StartApp()
    {
        AppController = Resolve<AppController>();
        FeedsController = Resolve<FeedsController>();
        SettingsController = Resolve<SettingsController>();
        AppController.Start();
        Shell = new((MockShellView)AppController.MainView);
    }

    public void Dispose()
    {
        OnDispose();
        Context.Dispose();
    }

    protected virtual void OnDispose() { }
}