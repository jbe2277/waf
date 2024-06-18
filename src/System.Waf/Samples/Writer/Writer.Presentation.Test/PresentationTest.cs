using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Waf.UnitTesting.Mocks;
using Test.Writer.Applications;
using Waf.Writer.Applications.Controllers;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Presentation.Services;

namespace Test.Writer.Presentation
{
    [TestClass]
    public abstract class PresentationTest : ApplicationsTest
    {
        // List of exports which must use the real implementation instead of the mock (integration test)
        private static readonly Type[] exportNames = [
            typeof(IRichTextDocumentType), typeof(RichTextDocumentType),
            typeof(IRichTextDocument), typeof(RichTextDocument),
            typeof(IXpsExportDocumentType), typeof(XpsExportDocumentType)
        ];

        protected PresentationTest()
        {
            LogManager.LogFactory.Dispose();  // Disable logging in unit tests
        }

        protected override void OnCatalogInitialize(AggregateCatalog catalog)
        {
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockMessageService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleController).Assembly));
            catalog.Catalogs.Add(new FilteredCatalog(new AssemblyCatalog(typeof(ApplicationsTest).Assembly), x => !IsOneOfContractNames(x)));
            catalog.Catalogs.Add(new FilteredCatalog(new AssemblyCatalog(typeof(RichTextDocument).Assembly), IsOneOfContractNames));

            static bool IsOneOfContractNames(ComposablePartDefinition d) => d.ExportDefinitions.Any(x => exportNames.Any(y => y.FullName == x.ContractName));
        }
    }
}
