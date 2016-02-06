using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.UnitTesting;

namespace Test.Waf.Applications
{
    [TestClass]
    public class AsyncDelegateCommandTest
    {
        [TestMethod]
        public void CanExecuteDuringAsyncExecute()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new AsyncDelegateCommand((Func<Task>)null));

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
            bool canExecute = false;
            bool executeCalled = false;

            var command = new AsyncDelegateCommand(() =>
            {
                executeCalled = true;
                return tcs.Task;
            }, () => canExecute);

            Assert.IsFalse(command.CanExecute(null));
            command.Execute(null);
            Assert.IsFalse(executeCalled);

            canExecute = true;
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

        [TestMethod]
        public void RaiseCanExecuteChangedTest()
        {
            bool executed = false;
            bool canExecute = false;
            AsyncDelegateCommand command = new AsyncDelegateCommand(() => { executed = false; return Task.FromResult((object)null); }, () => canExecute);

            Assert.IsFalse(command.CanExecute(null));
            canExecute = true;
            Assert.IsTrue(command.CanExecute(null));

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());

            Assert.IsFalse(executed);
        }
    }
}
