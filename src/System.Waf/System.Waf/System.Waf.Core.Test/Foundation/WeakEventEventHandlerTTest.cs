using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventEventHandlerTTest : WeakEventTestBase<WeakEventEventHandlerTTest.Manager, WeakEventEventHandlerTTest.Publisher, WeakEventEventHandlerTTest.Subscriber>
    {
        [TestMethod]
        public void WeakEventAddArgumentException()
        {
            var publisher = new Publisher();
            var subscriber = new Subscriber();
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.EventHandler<PropertyChangedEventArgs>.Add((Publisher)null!, subscriber.Handler, (s, h) => s.Event1 += h, (s, h) => s.Event1 -= h));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.EventHandler<PropertyChangedEventArgs>.Add(publisher, null!, (s, h) => s.Event1 += h, (s, h) => s.Event1 -= h));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.EventHandler<PropertyChangedEventArgs>.Add(publisher, subscriber.Handler, null!, (s, h) => s.Event1 -= h));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.EventHandler<PropertyChangedEventArgs>.Add(publisher, subscriber.Handler, (s, h) => s.Event1 += h, null!));
        }

        public class Manager : IManager<Publisher, Subscriber>
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Publisher publisher, Subscriber subscriber) => Proxy = WeakEvent.EventHandler<PropertyChangedEventArgs>.Add(publisher, subscriber.Handler, (s, h) => s.Event1 += h, (s, h) => s.Event1 -= h);
        }

        public class Publisher : IPublisher
        {
            private static readonly PropertyChangedEventArgs args = new PropertyChangedEventArgs("Test");
            private EventHandler<PropertyChangedEventArgs>? event1;

            public int EventHandlerCount { get; private set; }

            public event EventHandler<PropertyChangedEventArgs>? Event1
            {
                add
                {
                    event1 += value;
                    EventHandlerCount++;
                }
                remove
                {
                    event1 -= value;
                    EventHandlerCount--;
                }
            }

            public void RaiseEvent() => event1?.Invoke(this, args);
        }

        public class Subscriber : ISubscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, PropertyChangedEventArgs e) => HandlerCallCount++;
        }
    }
}
