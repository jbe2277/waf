using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Presentation.Converters;
using System.Waf.UnitTesting;
using System.Windows;

namespace Test.Waf.Presentation.Converters
{
    [TestClass]
    public class NullToVisibilityConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = NullToVisibilityConverter.Default;

            Assert.AreEqual(Visibility.Visible, converter.Convert(new object(), null, null, null));
            Assert.AreEqual(Visibility.Visible, converter.Convert("World", null, null, null));
            Assert.AreEqual(Visibility.Collapsed, converter.Convert(null, null, null, null));

            Assert.AreEqual(Visibility.Collapsed, converter.Convert(new object(), null, "Invert", null));
            Assert.AreEqual(Visibility.Visible, converter.Convert(null, null, "InVerT", null));

            AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(Visibility.Visible, null, null, null));
        }
    }
}
