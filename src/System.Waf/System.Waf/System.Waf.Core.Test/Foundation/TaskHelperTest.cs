using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Task.Delay(1).NoWait();
            await Task.Delay(5);
        }
    }
}
