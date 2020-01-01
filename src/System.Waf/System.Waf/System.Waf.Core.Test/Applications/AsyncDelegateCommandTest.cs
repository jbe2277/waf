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
            AssertHelper.ExpectedException<ArgumentNullException>(() => new AsyncDelegateCommand((Func<Task>)null!));

            var tcs = new TaskCompletionSource<object?>();
            var executeCalled = false;

            var command = new AsyncDelegateCommand(() =>
            {
                executeCalled = true;
                return tcs.Task;
            });

            Assert.IsTrue(command.CanExecute(null));
            command.Execute(null);

            executeCalled = false;
            Assert.IsFalse(command.CanExecute(null));
            command.Execute(null);
            Assert.IsFalse(executeCalled);

            tcs.SetResult(null);
            Assert.IsTrue(command.CanExecute(null));
            command.Execute(null);
            Assert.IsTrue(executeCalled);
        }

        [TestMethod]
        public void CanExecuteDuringAsyncExecute2()
        {
            var tcs = new TaskCompletionSource<object?>();
            var canExecute = false;
            var executeCalled = false;

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

            executeCalled = false;
            Assert.IsFalse(command.CanExecute(null));
            command.Execute(null);
            Assert.IsFalse(executeCalled);

            tcs.SetResult(null);
            Assert.IsTrue(command.CanExecute(null));
            command.Execute(null);
            Assert.IsTrue(executeCalled);
        }

        [TestMethod]
        public void CanExecuteDuringAsyncExecuteWithParameter()
        {
            var tcs = new TaskCompletionSource<object?>();
            string? commandParameter = null;

            var command = new AsyncDelegateCommand(p =>
            {
                commandParameter = (string?)p;
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
            var executed = false;
            var canExecute = false;
            var command = new AsyncDelegateCommand(() => { executed = true; return Task.CompletedTask; }, () => canExecute);

            Assert.IsFalse(command.CanExecute(null));
            canExecute = true;
            Assert.IsTrue(command.CanExecute(null));

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());

            AssertHelper.CanExecuteChangedEvent(command, () => command.Execute(null), 2, ExpectedChangedCountMode.Exact);  // Because during execution CanExecute returns false
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void DisabledCommandTest()
        {
            var disabledCommand1 = AsyncDelegateCommand.DisabledCommand;
            var disabledCommand2 = AsyncDelegateCommand.DisabledCommand;
            Assert.AreSame(disabledCommand1, disabledCommand2);
            Assert.IsFalse(disabledCommand1.CanExecute(null));
            disabledCommand1.Execute(null); // Nothing happens
        }
    }
}
