using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Waf.UnitTesting.Mocks;
using Test.InformationManager.Common.Domain;

namespace Test.InformationManager.Common.Applications
{
    [TestClass]
    public abstract class ApplicationsTest : DomainTest
    {
        public CompositionContainer Container { get; private set; }

        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();
            
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ApplicationsTest).Assembly));

            OnCatalogInitialize(catalog);

            Container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            var batch = new CompositionBatch();
            batch.AddExportedValue(Container);
            Container.Compose(batch);
        }

        protected override void OnTestCleanup()
        {
            Container?.Dispose();
            base.OnTestCleanup();
        }

        protected virtual void OnCatalogInitialize(AggregateCatalog catalog) { }
    }
}
