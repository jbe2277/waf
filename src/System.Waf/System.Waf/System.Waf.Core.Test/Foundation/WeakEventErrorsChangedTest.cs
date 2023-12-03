using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.ComponentModel;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventErrorsChangedTest : WeakEventTestBase<WeakEventErrorsChangedTest.Manager, WeakEventErrorsChangedTest.Publisher, WeakEventErrorsChangedTest.Subscriber>
    {
        [TestMethod]
        public void WeakEventAddArgumentException()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.ErrorsChanged.Add(null!, (s, h) => { }));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.ErrorsChanged.Add(new Publisher(), null!));
        }

        public class Manager : IManager<Publisher, Subscriber>
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Publisher publisher, Subscriber subscriber) => Proxy = WeakEvent.ErrorsChanged.Add(publisher, subscriber.Handler);
        }

        public class Publisher : IPublisher, INotifyDataErrorInfo
        {
            private static readonly DataErrorsChangedEventArgs args = new("Test");
            private EventHandler<DataErrorsChangedEventArgs>? errorsChanged;

            public int EventHandlerCount { get; private set; }

            public bool HasErrors => throw new NotImplementedException();

            public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
            {
                add
                {
                    errorsChanged += value;
                    EventHandlerCount++;
                }
                remove
                {
                    errorsChanged -= value;
                    EventHandlerCount--;
                }
            }

            public void RaiseEvent() => errorsChanged?.Invoke(this, args);

            public IEnumerable GetErrors(string? propertyName) => throw new NotImplementedException();
        }

        public class Subscriber : ISubscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, DataErrorsChangedEventArgs e) => HandlerCallCount++;
        }
    }
}
