using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Applications.Services;

namespace Test.Writer.Applications.Services;

[TestClass]
public class ShellServiceTest : ApplicationsTest
{
    [TestMethod]
    public void ZoomCommandsDisabled()
    {
        var shellService = Get<IShellService>();

        Assert.IsFalse(shellService.ActiveZoomCommands.ZoomInCommand.CanExecute(null));
        Assert.IsFalse(shellService.ActiveZoomCommands.ZoomOutCommand.CanExecute(null));
        Assert.IsFalse(shellService.ActiveZoomCommands.FitToWidthCommand.CanExecute(null));

        shellService.ActiveZoomCommands.Zoom = 3;
        Assert.AreEqual(1, shellService.ActiveZoomCommands.Zoom);
    }
}
