using System.ComponentModel.Composition.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.Common.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Controllers;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using System.IO;
using System;

namespace Test.InformationManager.Infrastructure.Modules.Applications
{
    [TestClass]
    public abstract class InfrastructureTest : ApplicationsTest
    {
        public TestContext TestContext { get; set; }

        
        protected override void OnCatalogInitialize(AggregateCatalog catalog)
        {
            base.OnCatalogInitialize(catalog);
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleController).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(InfrastructureTest).Assembly));
        }

        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();
            var environmentService = Container.GetExportedValue<MockEnvironmentService>();
            environmentService.DataDirectory = Path.Combine(Environment.CurrentDirectory, "Data");
        }
    }
}
