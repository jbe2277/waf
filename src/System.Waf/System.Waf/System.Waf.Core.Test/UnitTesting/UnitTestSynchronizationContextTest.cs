using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.UnitTesting;

namespace Test.Waf.UnitTesting
{
    [TestClass]
    public class UnitTestSynchronizationContextTest
    {
        [TestMethod]
        public void CreateAndDisposingTest()
        {
            Assert.IsNull(SynchronizationContext.Current);
            using (var context = UnitTestSynchronizationContext.Create())
            {
                Assert.AreEqual(context, SynchronizationContext.Current);
                Assert.AreEqual(context, UnitTestSynchronizationContext.Current);

                using (var nestedContext = (UnitTestSynchronizationContext)context.CreateCopy())
                {
                    SynchronizationContext.SetSynchronizationContext(nestedContext);
                    Assert.AreEqual(nestedContext, SynchronizationContext.Current);
                    Assert.AreEqual(nestedContext, UnitTestSynchronizationContext.Current);
                }

                Assert.AreEqual(context, SynchronizationContext.Current);
            }
            Assert.IsNull(SynchronizationContext.Current);
        }

        [TestMethod]
        public void SendTest()
        {
            int actionCallCount = 0;
            var context = UnitTestSynchronizationContext.Create();
            
            context.Send(state => actionCallCount++, null);
            Assert.AreEqual(1, actionCallCount);

            AssertHelper.ExpectedException<ArgumentNullException>(() => context.Send(null, null));

            context.Dispose();
            context.Send(state => actionCallCount++, null);
            Assert.AreEqual(1, actionCallCount);
        }

        [TestMethod]
        public void PostAndWaitTest()
        {
            int actionCallCount = 0;
            var context = UnitTestSynchronizationContext.Create();

            Func<Task> asyncAction = async () =>
            {
                await Task.Delay(1);
                Interlocked.Increment(ref actionCallCount);
            };

            var task = asyncAction();
            Assert.AreEqual(0, actionCallCount);

            task.Wait(context);
            Assert.AreEqual(1, actionCallCount);

            task = asyncAction();
            context.Wait(TimeSpan.FromMilliseconds(50));
            Assert.AreEqual(2, actionCallCount);

            context.Dispose();
            Assert.AreEqual(2, actionCallCount);
        }

        [TestMethod]
        public void PostAndWaitExceptionTest()
        {
            using (var context = UnitTestSynchronizationContext.Create())
            {
                AssertHelper.ExpectedException<ArgumentNullException>(() => context.Post(null, null));
                AssertHelper.ExpectedException<ArgumentNullException>(() => context.Wait(null));
            }
        }
    }
}
