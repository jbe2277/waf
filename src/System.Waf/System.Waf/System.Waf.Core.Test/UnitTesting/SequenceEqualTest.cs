using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.UnitTesting;

namespace Test.Waf.UnitTesting
{
    [TestClass]
    public class SequenceEqualTest
    {
        [TestMethod]
        public void SequenceEqual()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => AssertHelper.SequenceEqual<object>(null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => AssertHelper.SequenceEqual(new[] { "a" }, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => AssertHelper.SequenceEqual(null, new[] { "a" }));

            AssertHelper.SequenceEqual(new[] { 1, 2, 3 }, new[] { 1, 2, 3 });
            AssertHelper.SequenceEqual(new[] { "a", "b", "c" }, new[] { "a", "b", "c" });
            AssertHelper.SequenceEqual(new[] { "a", "b", "c" }, new[] { "A", "B", "C" }, StringComparer.OrdinalIgnoreCase);

            var assertException = AssertHelper.ExpectedException<AssertException>(() => AssertHelper.SequenceEqual(new[] { 1, 2, 3 }, new[] { 1, 2 }));
            StringAssert.Contains(assertException.Message, "[1, 2, 3]");
            StringAssert.Contains(assertException.Message, "[1, 2]");
            assertException = AssertHelper.ExpectedException<AssertException>(() => AssertHelper.SequenceEqual(new[] { 1, 2, 3 }, new[] { 1, 2, 4 }));
            StringAssert.Contains(assertException.Message, "[1, 2, 3]");
            StringAssert.Contains(assertException.Message, "[1, 2, 4]");
        }
    }
}
