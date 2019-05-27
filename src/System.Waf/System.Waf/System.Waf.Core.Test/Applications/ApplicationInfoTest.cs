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
            
            string productName = ApplicationInfo.ProductName;
            string version = ApplicationInfo.Version;
            string company = ApplicationInfo.Company;
            string copyright = ApplicationInfo.Copyright;
            Assert.IsNotNull(ApplicationInfo.ApplicationPath);

            // The second time it returns the cached values

            productName = ApplicationInfo.ProductName;
            version = ApplicationInfo.Version;
            company = ApplicationInfo.Company;
            copyright = ApplicationInfo.Copyright;
            Assert.IsNotNull(ApplicationInfo.ApplicationPath);
        }
    }
}
