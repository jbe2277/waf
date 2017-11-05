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
                ResourceHelper.GetPackUri("Resources/ConverterResources.xaml", "MyAssembly").ToString());

            Assert.AreEqual("pack://application:,,,/System.Waf.Wpf.Test;Component/Resources/ConverterResources.xaml",
                ResourceHelper.GetPackUri("Resources/ConverterResources.xaml", typeof(ResourceHelperTest).Assembly).ToString());
        }
    }
}
