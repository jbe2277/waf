using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using System.Windows.Input;

namespace Test.Waf.UnitTesting
{
    [TestClass]
    public class CanExecuteChangedEventTest
    {
        [TestMethod]
        public void CommandCanExecuteChangedTest1()
        {
            var command = new DelegateCommand(() => { });

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());
            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged(), 1, ExpectedChangedCountMode.Exact);
            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged(), 0, ExpectedChangedCountMode.AtLeast);
            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged(), 2, ExpectedChangedCountMode.AtMost);


            AssertHelper.ExpectedException<ArgumentNullException>(
                () => AssertHelper.CanExecuteChangedEvent(null!, () => command.RaiseCanExecuteChanged()));
            
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => AssertHelper.CanExecuteChangedEvent(command, null!));

            AssertHelper.ExpectedException<ArgumentOutOfRangeException>(
                () => AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged(), -1, ExpectedChangedCountMode.Exact));
        }

        [TestMethod]
        public void CommandCanExecuteChangedTest2()
        {
            var command = new DelegateCommand(() => { });
            
            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.CanExecuteChangedEvent(command, () => { }));

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.CanExecuteChangedEvent(command, () =>
                {
                    command.RaiseCanExecuteChanged();
                    command.RaiseCanExecuteChanged();
                }));

            
            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged(), 0, ExpectedChangedCountMode.Exact));

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged(), 2, ExpectedChangedCountMode.Exact));

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged(), 2, ExpectedChangedCountMode.AtLeast));

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged(), 0, ExpectedChangedCountMode.AtMost));
        }

        [TestMethod]
        public void WrongEventSenderTest()
        {
            var command = new WrongCommand();

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged()));
        }


        private class WrongCommand : ICommand
        {
            public event EventHandler? CanExecuteChanged;


            public bool CanExecute(object? parameter)
            {
                throw new NotImplementedException();
            }

            public void Execute(object? parameter)
            {
                throw new NotImplementedException();
            }

            public void RaiseCanExecuteChanged()
            {
                CanExecuteChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
