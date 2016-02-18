using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Applications.Services;
using System.Threading;
using System.Globalization;

namespace Test.Writer.Applications.Services
{
    [TestClass]
    public class ShellServiceTest : TestClassBase
    {
        [TestMethod]
        public void ZoomCommandsDisabled()
        {
            var shellService = Container.GetExportedValue<IShellService>();

            Assert.IsFalse(shellService.ActiveZoomCommands.ZoomInCommand.CanExecute(null));
            Assert.IsFalse(shellService.ActiveZoomCommands.ZoomOutCommand.CanExecute(null));
            Assert.IsFalse(shellService.ActiveZoomCommands.FitToWidthCommand.CanExecute(null));

            shellService.ActiveZoomCommands.Zoom = 3;
            Assert.AreEqual(1, shellService.ActiveZoomCommands.Zoom);
        }
    }
}
