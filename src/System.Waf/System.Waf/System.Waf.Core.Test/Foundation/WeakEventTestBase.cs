using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Foundation;

namespace Test.Waf.Foundation
{
    public interface IManager<TPublisher, TSubscriber>
        where TPublisher : class, IPublisher, new()
        where TSubscriber : class, ISubscriber, new()
    {
        IWeakEventProxy? Proxy { get; }

        void Add(TPublisher publisher, TSubscriber subscriber);
    }

    public interface IPublisher
    {
        int EventHandlerCount { get; }
        void RaiseEvent();
    }

    public interface ISubscriber
    {
        int HandlerCallCount { get; }
    }

    public class WeakEventTestBase<TManager, TPublisher, TSubscriber>
        where TManager : class, IManager<TPublisher, TSubscriber>, new()
        where TPublisher : class, IPublisher, new()
        where TSubscriber : class, ISubscriber, new()
    {
        [TestMethod]
        public void WeakEvent1() => WeakEvent1Core(1, false);

        [TestMethod]
        public void WeakEvent1B() => WeakEvent1Core(5, false);

        [TestMethod]
        public void WeakEvent1C() => WeakEvent1Core(1, true);

        private static void WeakEvent1Core(int addCount, bool removeTwice)
        {
            var publisher = new TPublisher();
            var (weakManager, _, weakSubscriber) = WeakEventHandlerCore(null, publisher, null, addCount: addCount, removeTwice: removeTwice);
            GC.Collect();
            Assert.IsFalse(weakManager.TryGetTarget(out _));
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));
            Assert.AreEqual(addCount, publisher.EventHandlerCount);
            publisher.RaiseEvent();
            Assert.AreEqual(0, publisher.EventHandlerCount);
            publisher.RaiseEvent();
            Assert.AreEqual(0, publisher.EventHandlerCount);
        }

        [TestMethod]
        public void WeakEvent2()
        {
            var subscriber = new TSubscriber();
            var (weakManager, weakPublisher, _) = WeakEventHandlerCore(null, null, subscriber);
            GC.Collect();
            Assert.IsFalse(weakManager.TryGetTarget(out _));
            Assert.IsFalse(weakPublisher.TryGetTarget(out _));
        }

        [TestMethod]
        public void WeakEvent3()
        {
            var manager = new TManager();
            var (_, weakPublisher, weakSubscriber) = WeakEventHandlerCore(manager, null, null);
            GC.Collect();
            Assert.IsFalse(weakPublisher.TryGetTarget(out _));
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));
        }

        [TestMethod]
        public void WeakEventRemove()
        {
            var publisher = new TPublisher();
            var (weakManager, _, weakSubscriber) = WeakEventHandlerCore(null, publisher, null, remove: true);
            GC.Collect();
            Assert.IsFalse(weakManager.TryGetTarget(out _));
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));
            Assert.AreEqual(0, publisher.EventHandlerCount);
        }

        [TestMethod]
        public void WeakEventPerformance()
        {
            var publisher = new TPublisher();
            WeakEventHandlerCore(null, publisher, null, raiseCount: 1_000_000);
        }

        private static (WeakReference<TManager>, WeakReference<TPublisher>, WeakReference<TSubscriber>) WeakEventHandlerCore(TManager? manager, TPublisher? publisher, TSubscriber? subscriber,
            int raiseCount = 1, int addCount = 1, bool remove = false, bool removeTwice = false)
        {
            manager ??= new TManager();
            publisher ??= new TPublisher();
            subscriber ??= new TSubscriber();
            Assert.AreEqual(0, publisher.EventHandlerCount);
            for (int i = 0; i < addCount; i++) manager.Add(publisher, subscriber);
            IWeakEventProxy? proxy1 = null;
            if (removeTwice)
            {
                proxy1 = manager.Proxy;
                manager.Add(publisher, subscriber);
            }
            Assert.AreEqual(removeTwice ? 2 : addCount, publisher.EventHandlerCount);
            proxy1?.Remove();
            proxy1?.Remove();

            GC.Collect();

            Assert.AreEqual(0, subscriber.HandlerCallCount);
            for (int i = 0; i < raiseCount; i++) publisher.RaiseEvent();
            Assert.AreEqual(addCount * raiseCount, subscriber.HandlerCallCount);
            if (remove)
            {
                var count = subscriber.HandlerCallCount;
                manager.Proxy!.Remove();
                publisher.RaiseEvent();
                Assert.AreEqual(count, subscriber.HandlerCallCount);
                manager.Proxy.Remove();
                publisher.RaiseEvent();
                Assert.AreEqual(count, subscriber.HandlerCallCount);
            }

            return (new(manager), new(publisher), new(subscriber));
        }
    }
}
