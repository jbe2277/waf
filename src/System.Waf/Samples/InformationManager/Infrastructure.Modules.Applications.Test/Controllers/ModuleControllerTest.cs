using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.Infrastructure.Modules.Applications.Controllers;
using Test.InformationManager.Infrastructure.Modules.Applications.Views;
using System.IO;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Controllers
{
    [TestClass]
    public class ModuleControllerTest : InfrastructureTest
    {
        [TestMethod]
        public void ControllerLifecycle()
        {
            var shellView = Container.GetExportedValue<MockShellView>();
            var moduleController = Container.GetExportedValue<ModuleController>();
            var documentController = Container.GetExportedValue<DocumentController>();
            if (File.Exists(documentController.PackagePath)) File.Delete(documentController.PackagePath);

            Assert.IsFalse(shellView.IsVisible);

            moduleController.Initialize();
            moduleController.Run();

            Assert.IsTrue(shellView.IsVisible);
            
            moduleController.Shutdown();
        }
    }
}
