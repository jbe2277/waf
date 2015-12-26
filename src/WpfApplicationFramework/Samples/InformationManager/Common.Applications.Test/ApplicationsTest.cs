using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.Common.Domain;
using System.Waf.UnitTesting.Mocks;

namespace Test.InformationManager.Common.Applications
{
    [TestClass]
    public abstract class ApplicationsTest : DomainTest
    {
        private CompositionContainer container;


        public CompositionContainer Container { get { return container; } }


        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();
            
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ApplicationsTest).Assembly));

            OnCatalogInitialize(catalog);

            container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);
        }

        protected override void OnTestCleanup()
        {
            if (container != null) { container.Dispose(); }

            base.OnTestCleanup();
        }

        protected virtual void OnCatalogInitialize(AggregateCatalog catalog) { }
    }
}
