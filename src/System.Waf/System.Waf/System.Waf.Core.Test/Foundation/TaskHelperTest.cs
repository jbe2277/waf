using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Waf.Foundation;

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
        public async Task NoWaitTest()
        {
            TaskHelper.NoWait(null);        // No NullReferenceException
            TaskHelper.NoWait(null, true);

            bool unobservedTaskException = false;
            EventHandler<UnobservedTaskExceptionEventArgs> handler = (sender, e) =>
            {
                unobservedTaskException = true;
            };
            TaskScheduler.UnobservedTaskException += handler;
            try
            {
                Task.Delay(1).NoWait();
                Task.Delay(1).NoWait(true);
                Task.Run(() => { throw new InvalidOperationException("Test"); }).NoWait(true);
                await Task.Delay(5);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                Assert.IsFalse(unobservedTaskException);
            }
            finally
            {
                TaskScheduler.UnobservedTaskException -= handler;
            }
        }
    }
}
