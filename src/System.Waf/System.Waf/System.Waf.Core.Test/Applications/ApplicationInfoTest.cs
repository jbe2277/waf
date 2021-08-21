using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;

namespace Test.Waf.Applications
{
    [TestClass]
    public class ApplicationInfoTest
    {
        [TestMethod]
        public void ApplicationInfoPropertiesTest()
        {
            // This doesn't work because the Visual Studio Unit Testing Framework
            // doesn't have an entry assembly
            
            _ = ApplicationInfo.ProductName;
            _ = ApplicationInfo.Version;
            _ = ApplicationInfo.Company;
            _ = ApplicationInfo.Copyright;
            Assert.IsNotNull(ApplicationInfo.ApplicationPath);

            // The second time it returns the cached values

            _ = ApplicationInfo.ProductName;
            _ = ApplicationInfo.Version;
            _ = ApplicationInfo.Company;
            _ = ApplicationInfo.Copyright;
            Assert.IsNotNull(ApplicationInfo.ApplicationPath);
        }
    }
}
