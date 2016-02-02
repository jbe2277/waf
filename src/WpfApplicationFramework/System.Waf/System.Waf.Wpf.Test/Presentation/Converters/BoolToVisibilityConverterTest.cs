using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Presentation.Converters;
using System.Windows;

namespace Test.Waf.Presentation.Converters
{
    [TestClass]
    public class BoolToVisibilityConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = BoolToVisibilityConverter.Default;
            
            Assert.AreEqual(Visibility.Visible, converter.Convert(true, null, null, null));
            Assert.AreEqual(Visibility.Collapsed, converter.Convert(false, null, null, null));
            Assert.AreEqual(Visibility.Collapsed, converter.Convert(null, null, null, null));

            Assert.AreEqual(Visibility.Collapsed, converter.Convert(true, null, "Invert", null));
            Assert.AreEqual(Visibility.Visible, converter.Convert(false, null, "invert", null));
            Assert.AreEqual(Visibility.Visible, converter.Convert(null, null, "InVerT", null));

            Assert.IsTrue((bool)converter.ConvertBack(Visibility.Visible, null, null, null));
            Assert.IsFalse((bool)converter.ConvertBack(Visibility.Collapsed, null, null, null));
            Assert.IsFalse((bool)converter.ConvertBack(Visibility.Hidden, null, null, null));

            Assert.IsFalse((bool)converter.ConvertBack(Visibility.Visible, null, "Invert", null));
            Assert.IsTrue((bool)converter.ConvertBack(Visibility.Collapsed, null, "invert", null));
            Assert.IsTrue((bool)converter.ConvertBack(Visibility.Hidden, null, "InVerT", null));
        }
    }
}
