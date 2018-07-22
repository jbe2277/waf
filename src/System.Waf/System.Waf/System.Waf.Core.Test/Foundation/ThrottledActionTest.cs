using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class ThrottledActionTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new ThrottledAction(null));
            var throttledAction = new ThrottledAction(() => { });
            Assert.IsFalse(throttledAction.IsRunning);
        }
        
        [TestMethod]
        public void InvokeMaxEveryDelayTimeTestWithoutSynchronizationContext()
        {
            Assert.IsNull(SynchronizationContext.Current);
            
            int actionCallCount = 0;
            var throttledAction = new ThrottledAction(() => Interlocked.Add(ref actionCallCount, 1), ThrottledActionMode.InvokeMaxEveryDelayTime, TimeSpan.FromMilliseconds(100));
            Assert.IsFalse(throttledAction.IsRunning);
            
            throttledAction.InvokeAccumulated();
            Assert.IsTrue(throttledAction.IsRunning);
            throttledAction.InvokeAccumulated();
            throttledAction.InvokeAccumulated();

            // Multiple calls of InvokeAccumulated within the delayTime should call the action just once.
            Task.Delay(200).Wait();
            Assert.AreEqual(1, actionCallCount);
            Assert.IsFalse(throttledAction.IsRunning);


            actionCallCount = 0;
            throttledAction.InvokeAccumulated();
            Task.Delay(10).Wait();
            throttledAction.InvokeAccumulated();
            Task.Delay(150).Wait();
            throttledAction.InvokeAccumulated();

            // Calls the action twice: First 2 InvokeAccumulated are within delayTime; Last is after delayTime.
            Task.Delay(200).Wait();
            Assert.AreEqual(2, actionCallCount);
            Assert.IsFalse(throttledAction.IsRunning);


            actionCallCount = 0;
            throttledAction.InvokeAccumulated();
            Task.Delay(10).Wait();
            throttledAction.Cancel();

            // Do not call the action because it is cancelled. 
            Task.Delay(200).Wait();
            Assert.AreEqual(0, actionCallCount);
            Assert.IsFalse(throttledAction.IsRunning);

            // Calling Cancel multiple time most not throw an exception
            throttledAction.Cancel();
            throttledAction.Cancel();
        }

        [TestMethod]
        public void InvokeOnlyIfIdleForDelayTimeWithoutSynchronizationContext()
        {
            Assert.IsNull(SynchronizationContext.Current);

            int actionCallCount = 0;
            var throttledAction = new ThrottledAction(() => Interlocked.Add(ref actionCallCount, 1), ThrottledActionMode.InvokeOnlyIfIdleForDelayTime, TimeSpan.FromMilliseconds(100));
            Assert.IsFalse(throttledAction.IsRunning);

            throttledAction.InvokeAccumulated();
            Assert.IsTrue(throttledAction.IsRunning);
            throttledAction.InvokeAccumulated();
            throttledAction.InvokeAccumulated();

            // Multiple calls of InvokeAccumulated within the delayTime should call the action just once.
            Task.Delay(200).Wait();
            Assert.AreEqual(1, actionCallCount);
            Assert.IsFalse(throttledAction.IsRunning);

            actionCallCount = 0;
            throttledAction.InvokeAccumulated();
            Task.Delay(60).Wait();
            throttledAction.InvokeAccumulated();
            Task.Delay(60).Wait();
            throttledAction.InvokeAccumulated();
            Task.Delay(60).Wait();
            throttledAction.InvokeAccumulated();

            // Calls just once: The waits between InvokeAccumulated are less than the idle (delay) time.
            Task.Delay(200).Wait();
            Assert.AreEqual(1, actionCallCount);
            Assert.IsFalse(throttledAction.IsRunning);
        }

        [TestMethod]
        public void InvokeMaxEveryDelayTimeTestWithSynchronizationContext()
        {
            using (var context = UnitTestSynchronizationContext.Create())
            {
                int actionCallCount = 0;
                var throttledAction = new ThrottledAction(() => Interlocked.Add(ref actionCallCount, 1), ThrottledActionMode.InvokeMaxEveryDelayTime, TimeSpan.FromMilliseconds(100));
                Assert.IsFalse(throttledAction.IsRunning);

                throttledAction.InvokeAccumulated();
                Assert.IsTrue(throttledAction.IsRunning);
                throttledAction.InvokeAccumulated();
                throttledAction.InvokeAccumulated();

                // As long the unit test synchronization context is not executed the actionCallCount must not be increased.
                Task.Delay(200).Wait();
                Assert.AreEqual(0, actionCallCount);

                // Execute the unit test synchronization context.
                context.WaitFor(() => actionCallCount > 0, TimeSpan.FromMilliseconds(200));
                Assert.AreEqual(1, actionCallCount);
                Assert.IsFalse(throttledAction.IsRunning);
            }
        }

        [TestMethod]
        public void InvokeOnlyIfIdleForDelayTimePerformanceTest()
        {
            int actionCallCount = 0;
            var throttledAction = new ThrottledAction(() => Interlocked.Add(ref actionCallCount, 1), ThrottledActionMode.InvokeOnlyIfIdleForDelayTime, TimeSpan.FromMilliseconds(10));
            for (int i = 0; i < 20000; i++)
            {
                throttledAction.InvokeAccumulated();
            }
            Task.Delay(100).Wait();
            Assert.AreEqual(1, actionCallCount);
        }
    }
}
