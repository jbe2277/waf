using System;
using System.Waf.Foundation;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subscriber = Test.Waf.Foundation.WeakEventEventHandlerTest.Subscriber;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventStaticEventHandlerTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            StaticPublisher.Reset();
        }

        [TestMethod]
        public void WeakEvent1() => WeakEvent1Core(1, false);

        [TestMethod]
        public void WeakEvent1B() => WeakEvent1Core(5, false);

        [TestMethod]
        public void WeakEvent1C() => WeakEvent1Core(1, true);

        private static void WeakEvent1Core(int addCount, bool removeTwice)
        {
            var (weakManager, weakSubscriber) = WeakEventHandlerCore(null, null, addCount: addCount, removeTwice: removeTwice);
            GC.Collect();
            Assert.IsFalse(weakManager.TryGetTarget(out _));
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));
            Assert.AreEqual(addCount, StaticPublisher.EventHandlerCount);
            StaticPublisher.RaiseEvent();
            Assert.AreEqual(0, StaticPublisher.EventHandlerCount);
            StaticPublisher.RaiseEvent();
            Assert.AreEqual(0, StaticPublisher.EventHandlerCount);
        }

        [TestMethod]
        public void WeakEvent2()
        {
            var subscriber = new Subscriber();
            var (weakManager, _) = WeakEventHandlerCore(null, subscriber);
            GC.Collect();
            Assert.IsFalse(weakManager.TryGetTarget(out _));
        }

        [TestMethod]
        public void WeakEvent3()
        {
            var manager = new Manager();
            var (_, weakSubscriber) = WeakEventHandlerCore(manager, null);
            GC.Collect();
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));
        }

        [TestMethod]
        public void WeakEventRemove()
        {
            var (weakManager, weakSubscriber) = WeakEventHandlerCore(null, null, remove: true);
            GC.Collect();
            Assert.IsFalse(weakManager.TryGetTarget(out _));
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));
            Assert.AreEqual(0, StaticPublisher.EventHandlerCount);
        }

        [TestMethod]
        public void WeakEventPerformance()
        {
            WeakEventHandlerCore(null, null, raiseCount: 1_000_000);
        }

        [TestMethod]
        public void WeakEventAddArgumentException()
        {
            var subscriber = new Subscriber();
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.StaticEventHandler.Add(null!, h => StaticPublisher.Event1 += h, h => StaticPublisher.Event1 -= h));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.StaticEventHandler.Add(subscriber.Handler, null!, h => StaticPublisher.Event1 -= h));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.StaticEventHandler.Add(subscriber.Handler, h => StaticPublisher.Event1 += h, null!));
        }

        [TestMethod]
        public void WeakEventRemoveTest()
        {
            var subscriber = new Subscriber();
            var proxy = WeakEvent.StaticEventHandler.Add(subscriber.Handler, h => StaticPublisher.Event1 += h, h => StaticPublisher.Event1 -= h);
            StaticPublisher.RaiseEvent();
            Assert.AreEqual(1, subscriber.HandlerCallCount);
            
            WeakEvent.TryRemove(ref proxy);
            StaticPublisher.RaiseEvent();
            Assert.AreEqual(1, subscriber.HandlerCallCount);
            Assert.IsNull(proxy);

            WeakEvent.TryRemove(ref proxy);
            StaticPublisher.RaiseEvent();
            Assert.AreEqual(1, subscriber.HandlerCallCount);
            Assert.IsNull(proxy);
        }

        private static (WeakReference<Manager>, WeakReference<Subscriber>) WeakEventHandlerCore(Manager? manager, Subscriber? subscriber,
            int raiseCount = 1, int addCount = 1, bool remove = false, bool removeTwice = false)
        {
            manager ??= new Manager();
            subscriber ??= new Subscriber();
            Assert.AreEqual(0, StaticPublisher.EventHandlerCount);
            for (int i = 0; i < addCount; i++) manager.Add(subscriber);
            IWeakEventProxy? proxy1 = null;
            if (removeTwice)
            {
                proxy1 = manager.Proxy;
                manager.Add(subscriber);
            }
            Assert.AreEqual(removeTwice ? 2 : addCount, StaticPublisher.EventHandlerCount);
            proxy1?.Remove();
            proxy1?.Remove();

            GC.Collect();

            Assert.AreEqual(0, subscriber.HandlerCallCount);
            for (int i = 0; i < raiseCount; i++) StaticPublisher.RaiseEvent();
            Assert.AreEqual(addCount * raiseCount, subscriber.HandlerCallCount);
            if (remove)
            {
                var count = subscriber.HandlerCallCount;
                manager.Proxy!.Remove();
                StaticPublisher.RaiseEvent();
                Assert.AreEqual(count, subscriber.HandlerCallCount);
                manager.Proxy!.Remove();
                StaticPublisher.RaiseEvent();
                Assert.AreEqual(count, subscriber.HandlerCallCount);
            }

            return (new(manager), new(subscriber));
        }

        public class Manager
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Subscriber subscriber) => Proxy = WeakEvent.StaticEventHandler.Add(subscriber.Handler, h => StaticPublisher.Event1 += h, h => StaticPublisher.Event1 -= h);
        }

        public static class StaticPublisher
        {
            private static EventHandler? event1;

            public static int EventHandlerCount { get; private set; }

            public static event EventHandler? Event1
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

            public static void RaiseEvent() => event1?.Invoke(null, EventArgs.Empty);

            public static void Reset()
            {
                EventHandlerCount = 0;
                event1 = null;
            }
        }
    }
}
