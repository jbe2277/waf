using System.ComponentModel.Composition.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.BookLibrary.Library.Applications;
using Waf.BookLibrary.Reporting.Applications.Controllers;

namespace Test.BookLibrary.Reporting.Applications
{
    [TestClass]
    public class ReportingTest : TestClassBase
    {
        protected override void OnCatalogInitialize(AggregateCatalog catalog)
        {
            base.OnCatalogInitialize(catalog);
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleController).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ReportingTest).Assembly));
        }
    }
}
