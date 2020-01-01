using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class CacheTest
    {
        [TestMethod]
        public void ValueAndIsDirtyTest()
        {
            int value = 3;
            int factoryCallCount = 0;
            var cache = new Cache<int>(() =>
            {
                factoryCallCount++;
                return value;
            });

            Assert.IsTrue(cache.IsDirty);
            Assert.AreEqual(0, factoryCallCount);

            Assert.AreEqual(3, cache.Value);
            Assert.IsFalse(cache.IsDirty);
            Assert.AreEqual(1, factoryCallCount);

            // Now the cached value is used.
            Assert.AreEqual(3, cache.Value);
            Assert.IsFalse(cache.IsDirty);
            Assert.AreEqual(1, factoryCallCount);

            value = 4;
            cache.SetDirty();
            Assert.IsTrue(cache.IsDirty);

            Assert.AreEqual(4, cache.Value);
            Assert.IsFalse(cache.IsDirty);
            Assert.AreEqual(2, factoryCallCount);

            AssertHelper.ExpectedException<ArgumentNullException>(() => new Cache<int>(null!));
        }
    }
}
