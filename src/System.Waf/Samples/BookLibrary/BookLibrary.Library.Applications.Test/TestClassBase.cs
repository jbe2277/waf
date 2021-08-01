using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.Controllers;

namespace Test.BookLibrary.Library.Applications
{
    [TestClass]
    public abstract class TestClassBase
    {
        public CompositionContainer? Container { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(TestClassBase).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleController).Assembly));

            OnCatalogInitialize(catalog);

            Container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            var batch = new CompositionBatch();
            batch.AddExportedValue(Container);
            Container.Compose(batch);
            
            OnInitialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            OnCleanup();
            Container?.Dispose();
        }

        public T Get<T>() => Container!.GetExportedValue<T>();

        public Lazy<T> GetLazy<T>() => new Lazy<T>(() => Container!.GetExportedValue<T>());

        protected virtual void OnCatalogInitialize(AggregateCatalog catalog) { }

        protected virtual void OnInitialize() { }

        protected virtual void OnCleanup() { }
    }
}
