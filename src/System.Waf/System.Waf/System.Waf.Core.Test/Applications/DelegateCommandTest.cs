using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            bool executed = false;
            DelegateCommand command = new DelegateCommand(() => executed = true);

            command.Execute(null);
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void ExecuteTest2()
        {
            bool executed = false;
            object commandParameter = null;
            DelegateCommand command = new DelegateCommand((object parameter) =>
            {
                executed = true;
                commandParameter = parameter;
            });

            object obj = new object();
            command.Execute(obj);
            Assert.IsTrue(executed);
            Assert.AreEqual(obj, commandParameter);
        }

        [TestMethod]
        public void ExecuteTest3()
        {
            bool executed = false;
            bool canExecute = true;
            DelegateCommand command = new DelegateCommand(() => executed = true, () => canExecute);

            command.Execute(null);
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void ExecuteTest4()
        {
            bool executed = false;
            bool canExecute = false;
            DelegateCommand command = new DelegateCommand(() => executed = true, () => canExecute);

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
            bool executed = false;
            bool canExecute = false;
            DelegateCommand command = new DelegateCommand(() => executed = true, () => canExecute);
            
            Assert.IsFalse(command.CanExecute(null));
            canExecute = true;
            Assert.IsTrue(command.CanExecute(null));

            command.RaiseCanExecuteChanged();
            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());
            
            Assert.IsFalse(executed);
        }

        [TestMethod]
        public void ConstructorParameterTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new DelegateCommand((Action)null));

            AssertHelper.ExpectedException<ArgumentNullException>(() => new DelegateCommand((Action<object>)null));
        }
    }
}
