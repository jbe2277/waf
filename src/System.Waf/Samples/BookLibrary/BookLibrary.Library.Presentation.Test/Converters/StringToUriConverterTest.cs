using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Library.Presentation.Converters;

namespace Test.BookLibrary.Library.Presentation.Converters
{
    [TestClass]
    public class StringToUriConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = new StringToUriConverter();

            var uri = (Uri)converter.Convert("harry.potter@hogwarts.edu", null, null, null)!;
            Assert.AreEqual("harry.potter@hogwarts.edu", uri.OriginalString);
            
            uri = (Uri)converter.Convert("wrongAddress", null, null, null)!;
            Assert.AreEqual("wrongAddress", uri.OriginalString);

            uri = (Uri)converter.Convert(null, null, null, null)!;
            Assert.AreEqual("", uri.OriginalString);
        }
        
        [TestMethod]
        public void ConvertBackTest()
        {
            var converter = new StringToUriConverter();

            Assert.AreEqual("harry.pott", converter.ConvertBack("harry.pott", null, null, null));

            Assert.AreEqual("harry.pott", converter.ConvertBack(new Uri("harry.pott", UriKind.RelativeOrAbsolute), null, null, null));
        }
    }
}
