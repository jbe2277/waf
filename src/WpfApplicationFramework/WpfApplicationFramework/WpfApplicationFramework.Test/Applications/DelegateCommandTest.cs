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

            AssertHelper.ExpectedException<InvalidOperationException>(() => command.Execute(null));
            Assert.IsFalse(executed);
        }

        [TestMethod]
        public void RaiseCanExecuteChangedTest()
        {
            bool executed = false;
            bool canExecute = false;
            DelegateCommand command = new DelegateCommand(() => executed = false, () => canExecute);
            
            Assert.IsFalse(command.CanExecute(null));
            canExecute = true;
            Assert.IsTrue(command.CanExecute(null));

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());
            
            Assert.IsFalse(executed);
        }

        [TestMethod]
        public void ConstructorParameterTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new DelegateCommand((Action)null));

            AssertHelper.ExpectedException<ArgumentNullException>(() => new DelegateCommand((Action<object>)null));
        }


        private class MockView
        {
            public bool CanExecuteChangedHandlerCalled { get; private set; }
            
            
            public void CanExecuteChangedHandler(object sender, EventArgs e)
            {
                CanExecuteChangedHandlerCalled = true;
            }

            public void Clear()
            {
                CanExecuteChangedHandlerCalled = false;
            }
        }
    }
}
