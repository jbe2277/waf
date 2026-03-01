using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.UnitTesting;

namespace Test.Waf.UnitTesting
{
    [TestClass]
    public class UnitTestSynchronizationContextMSTest
    {
        public UnitTestSynchronizationContext Context { get; private set; } = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            Context = UnitTestSynchronizationContext.Create();
            Assert.AreSame(Context, SynchronizationContext.Current);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.AreSame(Context, SynchronizationContext.Current);
            Context.Dispose();
        }

        [TestMethod]
        public void UnitTestSynchronizationContextUsage()
        {
            Assert.AreSame(Context, SynchronizationContext.Current);

            Assert.AreEqual(42, Test1Async().GetResult(Context));

            int secondPartCallCount = 0;
            Test2Async(() => secondPartCallCount++);
            Assert.AreEqual(0, secondPartCallCount);
            Context.WaitFor(() => secondPartCallCount == 1, TimeSpan.FromSeconds(1));
            Assert.AreEqual(1, secondPartCallCount);
        }

        private static async Task<int> Test1Async()
        {
            await Task.Delay(10);
            return 42;
        }

        private static async void Test2Async(Action secondPart)
        {
            await Task.Delay(10);
            secondPart();
        }
    }
}
