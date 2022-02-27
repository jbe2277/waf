using System.ComponentModel.Composition.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.AddressBook.Modules.Applications;
using Test.InformationManager.Common.Applications;
using Test.InformationManager.Infrastructure.Modules.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Controllers;

namespace Test.InformationManager.EmailClient.Modules.Applications;

[TestClass]
public abstract class EmailClientTest : ApplicationsTest
{
    protected override void OnCatalogInitialize(AggregateCatalog catalog)
    {
        base.OnCatalogInitialize(catalog);
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleController).Assembly));
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(InfrastructureTest).Assembly));
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(AddressBookTest).Assembly));
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(EmailClientTest).Assembly));
    }
}
