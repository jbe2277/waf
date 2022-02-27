using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.Infrastructure.Modules.Applications.Controllers;
using Test.InformationManager.Infrastructure.Modules.Applications.Views;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Controllers;

[TestClass]
public class ModuleControllerTest : InfrastructureTest
{
    [TestMethod]
    public void ControllerLifecycle()
    {
        var shellView = Get<MockShellView>();
        var moduleController = Get<ModuleController>();
        var documentController = Get<DocumentController>();
        if (File.Exists(documentController.PackagePath)) File.Delete(documentController.PackagePath);

        Assert.IsFalse(shellView.IsVisible);

        moduleController.Initialize();
        moduleController.Run();

        Assert.IsTrue(shellView.IsVisible);

        moduleController.Shutdown();
    }
}
