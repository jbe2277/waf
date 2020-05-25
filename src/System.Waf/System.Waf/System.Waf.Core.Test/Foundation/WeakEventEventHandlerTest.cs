using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventEventHandlerTest : WeakEventTestBase<WeakEventEventHandlerTest.Manager, WeakEventEventHandlerTest.Publisher, WeakEventEventHandlerTest.Subscriber>
    {
        [TestMethod]
        public void WeakEventAddArgumentException()
        {
            var publisher = new Publisher();
            var subscriber = new Subscriber();
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.EventHandler.Add((Publisher)null!, subscriber.Handler, (s, h) => s.Event1 += h, (s, h) => s.Event1 -= h));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.EventHandler.Add(publisher, null!, (s, h) => s.Event1 += h, (s, h) => s.Event1 -= h));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.EventHandler.Add(publisher, subscriber.Handler, null!, (s, h) => s.Event1 -= h));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.EventHandler.Add(publisher, subscriber.Handler, (s, h) => s.Event1 += h, null!));
        }

        public class Manager : IManager<Publisher, Subscriber>
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Publisher publisher, Subscriber subscriber) => Proxy = WeakEvent.EventHandler.Add(publisher, subscriber.Handler, (s, h) => s.Event1 += h, (s, h) => s.Event1 -= h);
        }

        public class Publisher : IPublisher
        {
            private EventHandler? event1;

            public int EventHandlerCount { get; private set; }

            public event EventHandler? Event1
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

            public void RaiseEvent() => event1?.Invoke(this, EventArgs.Empty);
        }

        public class Subscriber : ISubscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, EventArgs e) => HandlerCallCount++;
        }
    }
}
