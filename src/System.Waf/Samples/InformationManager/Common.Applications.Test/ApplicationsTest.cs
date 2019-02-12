using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ApplicationsTest).Assembly));

            OnCatalogInitialize(catalog);

            Container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            var batch = new CompositionBatch();
            batch.AddExportedValue(Container);
            Container.Compose(batch);
        }

        protected override void OnCleanup()
        {
            Container?.Dispose();
            base.OnCleanup();
        }

        public T Get<T>()
        {
            return Container.GetExportedValue<T>();
        }

        public Lazy<T> GetLazy<T>()
        {
            return new Lazy<T>(() => Container.GetExportedValue<T>());
        }

        protected virtual void OnCatalogInitialize(AggregateCatalog catalog) { }
    }
}
