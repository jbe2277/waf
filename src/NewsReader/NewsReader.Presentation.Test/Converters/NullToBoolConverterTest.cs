using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.NewsReader.Presentation.Converters;

namespace Test.Presentation.Converters;

[TestClass]
public class NullToBoolConverterTest
{
    [TestMethod]
    public void ConvertTest()
    {
        var c = new NullToBoolConverter();
        Assert.IsTrue((bool)c.Convert("Test", null, null, null)!);
        Assert.IsFalse((bool)c.Convert(null, null, null, null)!);

        Assert.IsFalse((bool)c.Convert("Test", null, "Invert", null)!);
        Assert.IsTrue((bool)c.Convert(null, null, "inverT", null)!);
    }
}
