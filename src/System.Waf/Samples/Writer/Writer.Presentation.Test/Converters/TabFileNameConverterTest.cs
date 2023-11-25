using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using System.Windows;
using Waf.Writer.Presentation.Converters;

namespace Test.Writer.Presentation.Converters;

[TestClass]
public class TabFileNameConverterTest
{
    [TestMethod]
    public void ConvertTest()
    {
        var converter = new TabFileNameConverter();

        Assert.AreEqual("Document 1.rtf", converter.Convert([ "Document 1.rtf", false ], null, null, null));
        Assert.AreEqual("Document 1.rtf*", converter.Convert([ "Document 1.rtf", true ], null, null, null));
        Assert.AreEqual("This is a document with a very long f...", converter.Convert([ "This is a document with a very long file name.rtf", false ], null, null, null));

        Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert([ new object(), new object() ], typeof(string), null, null));

        AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(null, null, null, null));
    }
}
