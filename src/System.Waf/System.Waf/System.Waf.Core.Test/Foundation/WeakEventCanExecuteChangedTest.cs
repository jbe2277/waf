using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Foundation;
using System.Waf.UnitTesting;
using System.Windows.Input;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventCanExecuteChangedTest : WeakEventTestBase<WeakEventCanExecuteChangedTest.Manager, WeakEventCanExecuteChangedTest.Publisher, WeakEventCanExecuteChangedTest.Subscriber>
    {
        [TestMethod]
        public void WeakEventAddArgumentException()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.CanExecuteChanged.Add(null!, (s, h) => { }));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.CanExecuteChanged.Add(new Publisher(), null!));
        }

        public class Manager : IManager<Publisher, Subscriber>
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Publisher publisher, Subscriber subscriber) => Proxy = WeakEvent.CanExecuteChanged.Add(publisher, subscriber.Handler);
        }

        public class Publisher : IPublisher, ICommand
        {
            private EventHandler? canExecuteChanged;

            public int EventHandlerCount { get; private set; }

            public event EventHandler? CanExecuteChanged
            {
                add
                {
                    canExecuteChanged += value;
                    EventHandlerCount++;
                }
                remove
                {
                    canExecuteChanged -= value;
                    EventHandlerCount--;
                }
            }

            public void RaiseEvent() => canExecuteChanged?.Invoke(this, EventArgs.Empty);

            public bool CanExecute(object parameter) => throw new NotImplementedException();

            public void Execute(object parameter) =>throw new NotImplementedException();
        }

        public class Subscriber : ISubscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, EventArgs e) => HandlerCallCount++;
        }
    }
}
