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
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.IsNotNull(ApplicationInfo.ApplicationPath);
#pragma warning restore CS0618 // Type or member is obsolete

            // The second time it returns the cached values

            _ = ApplicationInfo.ProductName;
            _ = ApplicationInfo.Version;
            _ = ApplicationInfo.Company;
            _ = ApplicationInfo.Copyright;
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.IsNotNull(ApplicationInfo.ApplicationPath);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
