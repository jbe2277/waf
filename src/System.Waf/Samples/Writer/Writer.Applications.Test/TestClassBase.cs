using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Controllers;

namespace Test.Writer.Applications
{
    [TestClass]
    public abstract class TestClassBase
    {
        private PrintController printController;

        protected CompositionContainer Container { get; private set; }

        [TestInitialize]
        public void TestInitialize()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleController).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(TestClassBase).Assembly));

            Container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(Container);
            Container.Compose(batch);
            
            Container.GetExportedValue<RichTextDocumentController>();
            Container.GetExportedValue<FileController>().Initialize();
            
            OnTestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Container?.Dispose();
            OnTestCleanup();
        }

        protected virtual void OnTestInitialize() { }

        protected virtual void OnTestCleanup() { }

        internal PrintController InitializePrintController()
        {
            printController = Container.GetExportedValue<PrintController>();
            printController.Initialize();
            return printController;
        }

        protected void ShutdownPrintController()
        {
            printController.Shutdown();
            printController = null;
        }
    }
}
