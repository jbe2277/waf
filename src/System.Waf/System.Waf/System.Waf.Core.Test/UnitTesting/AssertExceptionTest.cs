using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;

namespace Test.Waf.UnitTesting
{
    [TestClass]
    public class AssertExceptionTest
    {
        [TestMethod]
        public void AssertExceptionConstructorTest() 
        {
            new AssertException();
            new AssertException("message");
            new AssertException("message", null);
        }
    }
}
