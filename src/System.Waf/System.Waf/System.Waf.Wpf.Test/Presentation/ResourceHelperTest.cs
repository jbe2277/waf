using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Waf.Presentation;
using System.Waf.UnitTesting;
using System.Windows;
using System.Windows.Threading;

namespace Test.Waf.Presentation
{
    [TestClass]
    public class ResourceHelperTest
    {
        [TestMethod]
        public void GetPackUri()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.GetPackUri((string)null, "test"));
            AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.GetPackUri("", "test"));
            AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.GetPackUri("test", null));
            AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.GetPackUri("test", ""));
            AssertHelper.ExpectedException<ArgumentNullException>(() => ResourceHelper.GetPackUri((Assembly)null, "test"));
            AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.GetPackUri(typeof(ResourceHelperTest).Assembly, null));
            AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.GetPackUri(typeof(ResourceHelperTest).Assembly, ""));

            Assert.AreEqual("pack://application:,,,/MyAssembly;Component/Resources/ConverterResources.xaml", 
                ResourceHelper.GetPackUri("MyAssembly", "Resources/ConverterResources.xaml").ToString());

            Assert.AreEqual("pack://application:,,,/System.Waf.Wpf.Test;Component/Resources/ConverterResources.xaml",
                ResourceHelper.GetPackUri(typeof(ResourceHelperTest).Assembly, "Resources/ConverterResources.xaml").ToString());
        }

        [TestMethod]
        public void AddToApplicationResources()
        {
            var testAppDomain = AppDomain.CreateDomain("TestAppDomain", null, new AppDomainSetup { ApplicationBase = Environment.CurrentDirectory });
            try
            {
                var test = (ResourceHelperWpfTest)testAppDomain.CreateInstanceAndUnwrap(typeof(ResourceHelperWpfTest).Assembly.FullName, typeof(ResourceHelperWpfTest).FullName);
                test.AddToApplicationResources();
            }
            finally
            {
                AppDomain.Unload(testAppDomain);
            }
        }
        

        private class ResourceHelperWpfTest : MarshalByRefObject
        {
            public void AddToApplicationResources()
            {
                var app = new Application();
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => app.Run()));

                AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.AddToApplicationResources((string)null, "test"));
                AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.AddToApplicationResources("", "test"));
                AssertHelper.ExpectedException<ArgumentNullException>(() => ResourceHelper.AddToApplicationResources("test", null));
                AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.AddToApplicationResources("test", ""));
                AssertHelper.ExpectedException<ArgumentNullException>(() => ResourceHelper.AddToApplicationResources((Assembly)null, "test"));
                AssertHelper.ExpectedException<ArgumentNullException>(() => ResourceHelper.AddToApplicationResources(typeof(ResourceHelperTest).Assembly, null));
                AssertHelper.ExpectedException<ArgumentException>(() => ResourceHelper.AddToApplicationResources(typeof(ResourceHelperTest).Assembly, ""));

                Assert.AreEqual(0, app.Resources.MergedDictionaries.Count);
                ResourceHelper.AddToApplicationResources(typeof(ResourceHelperTest).Assembly,
                    "Presentation/Resources/BrushResources.xaml", "Presentation/Resources/LayoutResources.xaml");
                Assert.AreEqual(2, app.Resources.MergedDictionaries.Count);
                Assert.AreEqual("pack://application:,,,/System.Waf.Wpf.Test;Component/Presentation/Resources/BrushResources.xaml",
                    app.Resources.MergedDictionaries[0].Source.ToString());
                Assert.AreEqual("pack://application:,,,/System.Waf.Wpf.Test;Component/Presentation/Resources/LayoutResources.xaml",
                    app.Resources.MergedDictionaries[1].Source.ToString());

                app.Shutdown();
            }
        }
    }
}
