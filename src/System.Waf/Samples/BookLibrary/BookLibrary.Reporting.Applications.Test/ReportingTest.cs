using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition.Hosting;
using Test.BookLibrary.Library.Applications;
using Waf.BookLibrary.Reporting.Applications.Controllers;

namespace Test.BookLibrary.Reporting.Applications;

[TestClass]
public class ReportingTest : ApplicationsTest
{
    protected override void OnCatalogInitialize(AggregateCatalog catalog)
    {
        base.OnCatalogInitialize(catalog);
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleController).Assembly));
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(ReportingTest).Assembly));
    }
}
