using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Services;

namespace Test.BookLibrary.Library.Applications
{
    [TestClass]
    public abstract class TestClassBase
    {
        public CompositionContainer Container { get; private set; }


        [TestInitialize]
        public void TestInitialize()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(TestClassBase).Assembly));
            catalog.Catalogs.Add(new FilteredCatalog(new AssemblyCatalog(typeof(ModuleController).Assembly), 
                x => ExcludeTypes(x, typeof(IEntityController), typeof(IEntityService))));

            OnCatalogInitialize(catalog);

            Container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            var batch = new CompositionBatch();
            batch.AddExportedValue(Container);
            Container.Compose(batch);
            
            OnTestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            OnTestCleanup();
            Container?.Dispose();
        }

        protected virtual void OnCatalogInitialize(AggregateCatalog catalog) { }

        protected virtual void OnTestInitialize() { }

        protected virtual void OnTestCleanup() { }

        private bool ExcludeTypes(ComposablePartDefinition definition, params Type[] typesToExclude)
        {
            return !definition.ExportDefinitions.Select(x => x.ContractName).Intersect(typesToExclude.Select(x => x.FullName)).Any();
        }
    }
}
