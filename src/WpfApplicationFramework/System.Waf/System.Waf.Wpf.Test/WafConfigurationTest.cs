using System.Waf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Waf
{
    [TestClass]
    public class WafConfigurationTest
    {
        [TestMethod]
        public void IsInDesignModeTest()
        {
            Assert.IsFalse(WafConfiguration.IsInDesignMode);
        }
    }
}
