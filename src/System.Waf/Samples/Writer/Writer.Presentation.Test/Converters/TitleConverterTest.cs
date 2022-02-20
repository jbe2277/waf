using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Waf.UnitTesting;
using System.Windows;
using Waf.Writer.Presentation.Converters;

namespace Test.Writer.Presentation.Converters
{
    [TestClass]
    public class TitleConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = new TitleConverter();

            Assert.AreEqual("App Title", converter.Convert(new[] { "App Title", null }, null, null, CultureInfo.InvariantCulture));
            Assert.AreEqual("Document1.rtf - App Title", converter.Convert(new[] { "App Title", "Document1.rtf" }, null, null, CultureInfo.InvariantCulture));

            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(null, null, null, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(new[] { "Wrong" }, null, null, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(new object[] { 4, 2 }, null, null, null));

            AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(null, null, null, null));
        }
    }
}
