using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

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
            var shellView = new object();
            mockShellViewModel.View = shellView;
            Assert.AreEqual(shellView, shellService.ShellView);

            Assert.IsNull(shellService.ContentView);
            var contentView = new object();
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
            var newToolBarCommands = new[] 
            {
                new ToolBarCommand(emptyCommand, "Command 1"),
                new ToolBarCommand(emptyCommand, "Command 2")
            };

            Assert.IsFalse(mockShellViewModel.ToolBarCommands.Any());
            shellService.AddToolBarCommands(newToolBarCommands);
            AssertHelper.SequenceEqual(newToolBarCommands, mockShellViewModel.ToolBarCommands);
            shellService.ClearToolBarCommands();
            Assert.IsFalse(mockShellViewModel.ToolBarCommands.Any());
        }
    }
}
