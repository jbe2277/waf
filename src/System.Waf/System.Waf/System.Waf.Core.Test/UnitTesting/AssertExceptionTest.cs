using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Test.Waf.UnitTesting
{
    [TestClass]
    public class AssertExceptionTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void AssertExceptionConstructorTest() 
        {
            new AssertException();
            new AssertException("message");
            new AssertException("message", null);
        }
    }
}
