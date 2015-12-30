using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Controllers;

namespace Test.Writer.Applications
{
    [TestClass]
    public abstract class TestClassBase
    {
        private CompositionContainer container;
        private PrintController printController;
        private RichTextDocumentController richTextDocumentController;


        protected CompositionContainer Container => container;


        [TestInitialize]
        public void TestInitialize()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleController).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(TestClassBase).Assembly));

            container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);
            
            richTextDocumentController = Container.GetExportedValue<RichTextDocumentController>();
            Container.GetExportedValue<FileController>().Initialize();
            
            OnTestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            container?.Dispose();
            
            OnTestCleanup();
        }

        protected virtual void OnTestInitialize() { }

        protected virtual void OnTestCleanup() { }

        internal PrintController InitializePrintController()
        {
            printController = container.GetExportedValue<PrintController>();
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
