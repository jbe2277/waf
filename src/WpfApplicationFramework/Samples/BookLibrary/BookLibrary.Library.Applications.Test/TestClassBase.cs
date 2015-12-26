using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Services;

namespace Test.BookLibrary.Library.Applications
{
    [TestClass]
    public abstract class TestClassBase
    {
        private CompositionContainer container;


        public CompositionContainer Container { get { return container; } }


        [TestInitialize]
        public void TestInitialize()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(TestClassBase).Assembly));
            catalog.Catalogs.Add(new FilteredCatalog(new AssemblyCatalog(typeof(ModuleController).Assembly), 
                x => ExcludeTypes(x, typeof(IEntityController), typeof(IEntityService))));

            OnCatalogInitialize(catalog);

            container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);
            
            OnTestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            OnTestCleanup();

            if (container != null) { container.Dispose(); }
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
