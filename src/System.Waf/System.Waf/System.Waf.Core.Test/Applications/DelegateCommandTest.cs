using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.UnitTesting;

namespace Test.Waf.Applications
{
    [TestClass]
    public class DelegateCommandTest
    {
        [TestMethod]
        public void ExecuteTest()
        {
            var executed = false;
            var command = new DelegateCommand(() => executed = true);

            Assert.IsTrue(command.CanExecute(null));
            command.Execute(null);
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void ExecuteTest2()
        {
            var executed = false;
            object? commandParameter = null;
            var command = new DelegateCommand(parameter =>
            {
                executed = true;
                commandParameter = parameter;
            });

            var obj = new object();
            Assert.IsTrue(command.CanExecute(null));
            Assert.IsTrue(command.CanExecute(obj));

            command.Execute(obj);
            Assert.IsTrue(executed);
            Assert.AreSame(obj, commandParameter);
        }

        [TestMethod]
        public void ExecuteTest3()
        {
            var executed = false;
            var canExecute = true;
            var command = new DelegateCommand(() => executed = true, () => canExecute);

            Assert.IsTrue(command.CanExecute(null));
            command.Execute(null);
            Assert.IsTrue(executed);

            executed = false;
            canExecute = false;
            Assert.IsFalse(command.CanExecute(null));
            command.Execute(null);
            Assert.IsFalse(executed);
        }

        [TestMethod]
        public void DisabledCommandTest()
        {
            var disabledCommand1 = DelegateCommand.DisabledCommand;
            var disabledCommand2 = DelegateCommand.DisabledCommand;
            Assert.AreSame(disabledCommand1, disabledCommand2);
            Assert.IsFalse(disabledCommand1.CanExecute(null));
            disabledCommand1.Execute(null); // Nothing happens
        }

        [TestMethod]
        public void RaiseCanExecuteChangedTest()
        {
            var (executed, canExecute) = (false, false);
            var command = new DelegateCommand(() => executed = true, () => canExecute);
            
            Assert.IsFalse(command.CanExecute(null));
            canExecute = true;
            Assert.IsTrue(command.CanExecute(null));

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());
            
            Assert.IsFalse(executed);

            var (executed2, canExecute2) = (false, false);
            var command2 = new AsyncDelegateCommand(() => { executed2 = true; return Task.CompletedTask; }, () => canExecute2);

            AssertHelper.CanExecuteChangedEvent(command, () => DelegateCommand.RaiseCanExecuteChanged(command, command2));
            AssertHelper.CanExecuteChangedEvent(command2, () => DelegateCommand.RaiseCanExecuteChanged(command, command2));
        }

        [TestMethod]
        public void ConstructorParameterTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new DelegateCommand((Action)null!));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new DelegateCommand((Action<object?>)null!));
        }
    }
}
