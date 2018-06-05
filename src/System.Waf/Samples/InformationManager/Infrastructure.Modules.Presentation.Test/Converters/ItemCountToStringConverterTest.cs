using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Waf.UnitTesting;
using Waf.InformationManager.Infrastructure.Modules.Presentation.Converters;

namespace Test.InformationManager.Infrastructure.Modules.Presentation.Converters
{
    [TestClass]
    public class ItemCountToStringConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = new ItemCountToStringConverter();

            Assert.AreEqual("", converter.Convert(null, null, null, CultureInfo.InvariantCulture));
            Assert.AreEqual("  3", converter.Convert(3, null, null, CultureInfo.InvariantCulture));
            Assert.AreEqual("  33", converter.Convert(33, null, null, CultureInfo.InvariantCulture));
            
            AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(null, null, null, null));
        }
    }
}
