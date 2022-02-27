using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Presentation.Converters;
using System.Waf.UnitTesting;

namespace Test.InformationManager.EmailClient.Modules.Presentation.Converters;

[TestClass]
public class StringCollectionToStringConverterTest
{
    [TestMethod]
    public void ConvertTest()
    {
        var converter = new StringCollectionToStringConverter();

        Assert.AreEqual("abc", converter.Convert(new[] { "abc" }, null, null, null));
        Assert.AreEqual("abc; def; ghi", converter.Convert(new[] { "abc", "def", "ghi" }, null, null, null));

        AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(null, null, null, null));
    }
}
