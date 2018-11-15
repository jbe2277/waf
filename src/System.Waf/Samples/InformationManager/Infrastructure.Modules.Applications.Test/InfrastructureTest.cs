using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Test.InformationManager.Common.Applications;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Applications.Controllers;

namespace Test.InformationManager.Infrastructure.Modules.Applications
{
    [TestClass]
    public abstract class InfrastructureTest : ApplicationsTest
    {
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
