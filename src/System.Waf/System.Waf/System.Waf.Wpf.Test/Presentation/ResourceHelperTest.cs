using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Presentation;

namespace Test.Waf.Presentation
{
    [TestClass]
    public class ResourceHelperTest
    {
        [TestMethod]
        public void GetPackUri()
        {
            Assert.AreEqual("pack://application:,,,/MyAssembly;Component/Resources/ConverterResources.xaml", 
                ResourceHelper.GetPackUri("MyAssembly", "Resources/ConverterResources.xaml").ToString());

            Assert.AreEqual("pack://application:,,,/System.Waf.Wpf.Test;Component/Resources/ConverterResources.xaml",
                ResourceHelper.GetPackUri(typeof(ResourceHelperTest).Assembly, "Resources/ConverterResources.xaml").ToString());
        }
    }
}
