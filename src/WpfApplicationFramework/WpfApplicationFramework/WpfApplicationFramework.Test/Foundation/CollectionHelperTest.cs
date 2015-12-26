using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using System.Waf.Foundation;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class CollectionHelperTest
    {
        [TestMethod]
        public void GetNextElementOrDefaultTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => CollectionHelper.GetNextElementOrDefault(null, 5));

            int[] collection1 = new[] { 1, 2, 3, 4, 5 };
            Assert.AreEqual(2, CollectionHelper.GetNextElementOrDefault(collection1, 1));
            Assert.AreEqual(4, CollectionHelper.GetNextElementOrDefault(collection1, 3));
            Assert.AreEqual(0, CollectionHelper.GetNextElementOrDefault(collection1, 5));

            int[] collection2 = new[] { 1 };
            Assert.AreEqual(0, CollectionHelper.GetNextElementOrDefault(collection2, 1));

            int[] collection3 = new int[] { };
            AssertHelper.ExpectedException<ArgumentException>(() => CollectionHelper.GetNextElementOrDefault(collection3, 9));
        }
    }
}
