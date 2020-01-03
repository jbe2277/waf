using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.UnitTesting;

namespace Test.Waf.UnitTesting
{
    [TestClass]
    public class UnitTestSynchronizationContextExtensionsTest
    {
        [TestMethod]
        public void WaitTest()
        {
            using (var context = UnitTestSynchronizationContext.Create())
            {
                int actionCallCount = 0;
                async Task<int> AsyncAction()
                {
                    await Task.Delay(1);
                    Interlocked.Increment(ref actionCallCount);
                    return actionCallCount;
                }

                var task = AsyncAction();
                Assert.AreEqual(0, actionCallCount);

                task.Wait(context);
                Assert.AreEqual(1, actionCallCount);
            }

            AssertHelper.ExpectedException<ArgumentNullException>(() => UnitTestSynchronizationContextExtensions.Wait(Task.FromResult((object)null!), null!));
        }

        [TestMethod]
        public void GetResultTest()
        {
            using (var context = UnitTestSynchronizationContext.Create())
            {
                int actionCallCount = 0;
                async Task<int> AsyncAction()
                {
                    await Task.Delay(1);
                    Interlocked.Increment(ref actionCallCount);
                    return actionCallCount;
                }

                var task = AsyncAction();
                Assert.AreEqual(0, actionCallCount);

                var result = task.GetResult(context);
                Assert.AreEqual(1, actionCallCount);
                Assert.AreEqual(1, result);
            }

            AssertHelper.ExpectedException<ArgumentNullException>(() => UnitTestSynchronizationContextExtensions.GetResult(Task.FromResult((object)null!), null!));
        }

        [TestMethod]
        public void WaitForTest()
        {
            using (var context = UnitTestSynchronizationContext.Create())
            {
                bool flag = false;
                async void AsyncAction()
                {
                    await Task.Delay(10);
                    flag = true;
                }

                AsyncAction();
                Assert.IsFalse(flag);

                context.WaitFor(() => flag, TimeSpan.FromSeconds(1));
                Assert.IsTrue(flag);

                AssertHelper.ExpectedException<TimeoutException>(() => context.WaitFor(() => !flag, TimeSpan.FromMilliseconds(10)));

                AssertHelper.ExpectedException<ArgumentNullException>(() => UnitTestSynchronizationContextExtensions.WaitFor(context, null!, TimeSpan.FromMilliseconds(10)));
            }
            
            AssertHelper.ExpectedException<ArgumentNullException>(() => UnitTestSynchronizationContextExtensions.WaitFor(null!, () => true, TimeSpan.FromMilliseconds(10)));
        }
    }
}
