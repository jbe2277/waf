using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting;

namespace Test.Waf.Applications.Services
{
    [TestClass]
    public class SettingsErrorEventArgsTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new SettingsErrorEventArgs(null!, SettingsServiceAction.Open, "file"));
            _ = new SettingsErrorEventArgs(new InvalidOperationException("Test"), SettingsServiceAction.Open, null);
        }
    }
}
