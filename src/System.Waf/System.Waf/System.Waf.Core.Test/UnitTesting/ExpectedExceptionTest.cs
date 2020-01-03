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

        [TestMethod, ExpectedException(typeof(AssertException))]
        public void ExpectedExceptionTest2()
        {
            AssertHelper.ExpectedException<ArgumentException>(ThrowsArgumentNullException);
        }

        [TestMethod, ExpectedException(typeof(AssertException))]
        public void ExpectedExceptionTest3()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(DoNothing);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedExceptionTest4()
        {
            AssertHelper.ExpectedException<InvalidOperationException>(null!);
        }

        private static void ThrowsArgumentNullException()
        {
            throw new ArgumentNullException();
        }

        private static void DoNothing() { }
    }
}
