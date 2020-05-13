using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Foundation;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakHelperEventHandlerTest
    {
        [TestMethod]
        public void WeakEvent1()
        {
            var publisher = new Publisher();
            var (weakManager, _, weakSubscriber) = WeakEventHandlerCore(null, publisher, null);
            GC.Collect();
            Assert.IsFalse(weakManager.TryGetTarget(out _));
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));
            Assert.AreEqual(1, publisher.EventHandlerCount);
            publisher.RaiseEvent();
            Assert.AreEqual(0, publisher.EventHandlerCount);
            publisher.RaiseEvent();
            Assert.AreEqual(0, publisher.EventHandlerCount);
        }

        [TestMethod]
        public void WeakEvent2()
        {
            var subscriber = new Subscriber();
            var (weakManager, weakPublisher, _) = WeakEventHandlerCore(null, null, subscriber);
            GC.Collect();
            Assert.IsFalse(weakManager.TryGetTarget(out _));
            Assert.IsFalse(weakPublisher.TryGetTarget(out _));
        }

        [TestMethod]
        public void WeakEvent3()
        {
            var manager = new Manager();
            var (_, weakPublisher, weakSubscriber) = WeakEventHandlerCore(manager, null, null);
            GC.Collect();
            Assert.IsFalse(weakPublisher.TryGetTarget(out _));
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));
        }

        [TestMethod]
        public void WeakEventDeregister()
        {
            var publisher = new Publisher();
            var (weakManager, _, weakSubscriber) = WeakEventHandlerCore(null, publisher, null, deregister: true);
            GC.Collect();
            Assert.IsFalse(weakManager.TryGetTarget(out _));
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));
            Assert.AreEqual(0, publisher.EventHandlerCount);
        }

        [TestMethod]
        public void WeakEventPerformance()
        {
            var publisher = new Publisher();
            WeakEventHandlerCore(null, publisher, null, raiseCount: 1_000_000);
        }

        private (WeakReference<Manager>, WeakReference<Publisher>, WeakReference<Subscriber>) WeakEventHandlerCore(Manager? manager, Publisher? publisher, Subscriber? subscriber, 
            int raiseCount = 1, bool deregister = false)
        {
            manager ??= new Manager();
            publisher ??= new Publisher();
            subscriber ??= new Subscriber();
            Assert.AreEqual(0, publisher.EventHandlerCount);
            var proxy = manager.Register(publisher, subscriber);
            Assert.AreEqual(1, publisher.EventHandlerCount);

            Assert.AreEqual(0, subscriber.HandlerCallCount);
            for (int i = 0; i < raiseCount; i++) publisher.RaiseEvent();
            Assert.AreEqual(raiseCount, subscriber.HandlerCallCount);
            if (deregister)
            {
                var count = subscriber.HandlerCallCount;
                proxy.Deregister();
                publisher.RaiseEvent();
                Assert.AreEqual(count, subscriber.HandlerCallCount);
            }
            
            return (new WeakReference<Manager>(manager), new WeakReference<Publisher>(publisher), new WeakReference<Subscriber>(subscriber));
        }

        private class Manager
        {
            public IWeakEventProxy Register(Publisher publisher, Subscriber subscriber) => WeakHelper.EventHandler.Register(publisher, subscriber.Handler, (s, h) => s.Event1 += h, (s, h) => s.Event1 -= h);
        }

        private class Publisher
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

        private class Subscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, EventArgs e) => HandlerCallCount++;
        }
    }
}
