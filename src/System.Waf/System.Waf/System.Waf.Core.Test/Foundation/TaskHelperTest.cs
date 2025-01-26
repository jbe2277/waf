using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class TaskHelperTest
    {
        [TestMethod]
        public async Task RunTest()
        {
            await TaskHelper.Run(() => { }, TaskScheduler.Current);
            var result = await TaskHelper.Run(() => 42, TaskScheduler.Current);
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void RunWithCancellationTest()
        {
            using var context = UnitTestSynchronizationContext.Create();

            var task = TaskHelper.Run(() => 42, TaskScheduler.FromCurrentSynchronizationContext());
            var result = task.GetResult(context);
            Assert.AreEqual(42, result);

            bool called = false;
            var cts = new CancellationTokenSource();
            var task2 = TaskHelper.Run(() => called = true, TaskScheduler.FromCurrentSynchronizationContext(), cts.Token);
            cts.Cancel();
            AssertHelper.ExpectedException<TaskCanceledException>(() => task2.Wait(context));
            Assert.IsFalse(called);
        }

        [TestMethod]
        public async Task NoWaitTest()
        {
            TaskHelper.NoWait(null);        // No NullReferenceException
            TaskHelper.NoWait(null, true);

            bool unobservedTaskException = false;
            void Handler(object? sender, UnobservedTaskExceptionEventArgs e)
            {
                unobservedTaskException = true;
            }
            TaskScheduler.UnobservedTaskException += Handler;
            try
            {
                Task.Delay(1).NoWait();
                Task.Delay(1).NoWait(true);
                Task.Run(() => throw new InvalidOperationException("Test")).NoWait(true);
                await Task.Delay(5);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                Assert.IsFalse(unobservedTaskException);
            }
            finally
            {
                TaskScheduler.UnobservedTaskException -= Handler;
            }
        }
    }
}
