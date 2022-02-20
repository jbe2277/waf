using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Controllers;

namespace Test.Writer.Applications;

[TestClass]
public abstract class TestClassBase
{
    private PrintController? printController;

    protected CompositionContainer Container { get; private set; } = null!;

    [TestInitialize]
    public void Initialize()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var catalog = new AggregateCatalog();
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleController).Assembly));
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(TestClassBase).Assembly));

        Container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
        var batch = new CompositionBatch();
        batch.AddExportedValue(Container);
        Container.Compose(batch);

        Get<RichTextDocumentController>();
        Get<FileController>().Initialize();

        OnInitialize();
    }

    [TestCleanup]
    public void Cleanup()
    {
        Container?.Dispose();
        OnCleanup();
    }

    public T Get<T>() => Container.GetExportedValue<T>();

    public Lazy<T> GetLazy<T>() => new(() => Container.GetExportedValue<T>());

    protected virtual void OnInitialize() { }

    protected virtual void OnCleanup() { }

    internal PrintController InitializePrintController()
    {
        printController = Get<PrintController>();
        printController.Initialize();
        return printController;
    }

    protected void ShutdownPrintController() => printController = null;
}
