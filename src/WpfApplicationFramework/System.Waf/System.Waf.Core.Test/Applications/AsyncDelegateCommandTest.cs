using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace Test.Waf.Applications
{
    [TestClass]
    public class AsyncDelegateCommandTest
    {
        [TestMethod]
        public void CanExecuteDuringAsyncExecute()
        {
            var tcs = new TaskCompletionSource<object>();
            bool executeCalled = false;

            var command = new AsyncDelegateCommand(() =>
            {
                executeCalled = true;
                return tcs.Task;
            });
            Assert.IsTrue(command.CanExecute(null));
            command.Execute(null);
            Assert.IsFalse(command.CanExecute(null));
            tcs.SetResult(null);
            Assert.IsTrue(command.CanExecute(null));
            Assert.IsTrue(executeCalled);
        }

        [TestMethod]
        public void CanExecuteDuringAsyncExecute2()
        {
            var tcs = new TaskCompletionSource<object>();
            bool executeCalled = false;

            var command = new AsyncDelegateCommand(() =>
            {
                executeCalled = true;
                return tcs.Task;
            }, () => true);
            Assert.IsTrue(command.CanExecute(null));
            command.Execute(null);
            Assert.IsFalse(command.CanExecute(null));
            tcs.SetResult(null);
            Assert.IsTrue(command.CanExecute(null));
            Assert.IsTrue(executeCalled);
        }

        [TestMethod]
        public void CanExecuteDuringAsyncExecuteWithParameter()
        {
            var tcs = new TaskCompletionSource<object>();
            string commandParameter = null;

            var command = new AsyncDelegateCommand(p =>
            {
                commandParameter = (string)p;
                return tcs.Task;
            });
            Assert.IsTrue(command.CanExecute(null));
            command.Execute("test");
            Assert.IsFalse(command.CanExecute(null));
            tcs.SetResult(null);
            Assert.IsTrue(command.CanExecute(null));
            Assert.AreEqual("test", commandParameter);
        }
    }
}
