using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.Infrastructure.Modules.Applications.Controllers;
using Test.InformationManager.Infrastructure.Modules.Applications.Views;

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

            Assert.IsFalse(shellView.IsVisible);

            moduleController.Initialize();
            moduleController.Run();

            Assert.IsTrue(shellView.IsVisible);
            
            moduleController.Shutdown();
        }
    }
}
