using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services
{
    [TestClass]
    public class ToolBarCommandTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var emptyCommand = new DelegateCommand(() => { });
            
            AssertHelper.ExpectedException<ArgumentNullException>(() => new ToolBarCommand(null!, null!));
            AssertHelper.ExpectedException<ArgumentException>(() => new ToolBarCommand(emptyCommand, null!));

            var toolBarCommand1 = new ToolBarCommand(emptyCommand, "Command 1");
            Assert.AreEqual(emptyCommand, toolBarCommand1.Command);
            Assert.AreEqual("Command 1", toolBarCommand1.Text);
            Assert.AreEqual("", toolBarCommand1.ToolTip);

            var toolBarCommand2 = new ToolBarCommand(emptyCommand, "Command 2", "ToolTip 2");
            Assert.AreEqual("ToolTip 2", toolBarCommand2.ToolTip);
        }
    }
}
