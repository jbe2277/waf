using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class StringHelperTest
    {
        [TestMethod]
        public void TruncateTest()
        {
            Assert.AreSame("Hi", StringHelper.Truncate("Hi", 5));
            Assert.AreSame("Hello", StringHelper.Truncate("Hello", 5));
            Assert.AreEqual("Hello", StringHelper.Truncate("Hello World", 5));

            Assert.AreEqual("", StringHelper.Truncate("Hi", 0));
            Assert.IsNull(StringHelper.Truncate(null, 5));
            Assert.AreEqual("", StringHelper.Truncate("", 5));
            AssertHelper.ExpectedException<ArgumentOutOfRangeException>(() => StringHelper.Truncate("Hi", -5));
        }
    }
}
