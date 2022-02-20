using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Presentation.Converters;

namespace Test.Writer.Presentation.Converters;

[TestClass]
public class DoubleToZoomConverterTest
{
    [TestMethod]
    public void ConvertTest()
    {
        var converter = new DoubleToZoomConverter();
        Assert.AreEqual(75d, converter.Convert(0.75, null, null, null));
    }

    [TestMethod]
    public void ConvertBackTest()
    {
        var converter = new DoubleToZoomConverter();
        Assert.AreEqual(0.75, converter.ConvertBack(75d, null, null, null));
    }
}
