using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;
using System.Waf.UnitTesting;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using System.Waf.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services
{
    [TestClass]
    public class ShellServiceTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var lazyShellViewModel = new Lazy<IShellViewModel>(() => new MockShellViewModel());
            var shellService = new ShellService(lazyShellViewModel);

            var mockShellViewModel = (MockShellViewModel)lazyShellViewModel.Value;
            object shellView = new object();
            mockShellViewModel.View = shellView;
            Assert.AreEqual(shellView, shellService.ShellView);

            Assert.IsNull(shellService.ContentView);
            object contentView = new object();
            AssertHelper.PropertyChangedEvent(shellService, x => x.ContentView, () => shellService.ContentView = contentView);
            Assert.AreEqual(contentView, shellService.ContentView);
        }

        [TestMethod]
        public void ToolBarCommandsDelegation()
        {
            var lazyShellViewModel = new Lazy<IShellViewModel>(() => new MockShellViewModel());
            var shellService = new ShellService(lazyShellViewModel);

            var mockShellViewModel = (MockShellViewModel)lazyShellViewModel.Value;
            var emptyCommand = new DelegateCommand(() => { });
            var newToolBarCommands = new ToolBarCommand[] 
            {
                new ToolBarCommand(emptyCommand, "Command 1"),
                new ToolBarCommand(emptyCommand, "Command 2")
            };

            Assert.IsFalse(mockShellViewModel.ToolBarCommands.Any());
            shellService.AddToolBarCommands(newToolBarCommands);
            Assert.IsTrue(mockShellViewModel.ToolBarCommands.SequenceEqual(newToolBarCommands));
            shellService.ClearToolBarCommands();
            Assert.IsFalse(mockShellViewModel.ToolBarCommands.Any());
        }
    }
}
