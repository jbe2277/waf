using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;

namespace Test.Waf.UnitTesting
{
    [TestClass]
    public class ExpectedExceptionTest
    {
        [TestMethod]
        public void ExpectedExceptionTest1()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(ThrowsArgumentNullException);
        }

        [TestMethod]
        public void ExpectedExceptionTest2()
        {
            Assert.ThrowsExactly<AssertException>(() => AssertHelper.ExpectedException<ArgumentException>(ThrowsArgumentNullException));
        }

        [TestMethod]
        public void ExpectedExceptionTest3()
        {
            Assert.ThrowsExactly<AssertException>(() => AssertHelper.ExpectedException<ArgumentNullException>(DoNothing));
        }

        [TestMethod]
        public void ExpectedExceptionTest4()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => AssertHelper.ExpectedException<InvalidOperationException>(null!));
        }

        private static void ThrowsArgumentNullException()
        {
            throw new ArgumentNullException();
        }

        private static void DoNothing() { }
    }
}
