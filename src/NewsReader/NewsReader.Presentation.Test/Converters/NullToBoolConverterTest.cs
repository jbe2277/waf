using Waf.NewsReader.Presentation.Converters;
using Xunit;

namespace Test.Presentation.Converters;

public class NullToBoolConverterTest
{
    [Fact]
    public void ConvertTest()
    {
        var c = new NullToBoolConverter();
        Assert.True((bool)c.Convert("Test", null, null, null)!);
        Assert.False((bool)c.Convert(null, null, null, null)!);

        Assert.False((bool)c.Convert("Test", null, "Invert", null)!);
        Assert.True((bool)c.Convert(null, null, "inverT", null)!);
    }
}
