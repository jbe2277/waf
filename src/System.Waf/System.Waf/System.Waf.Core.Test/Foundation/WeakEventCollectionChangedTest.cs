using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Waf.Foundation;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventCollectionChangedTest
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
        public void WeakEventRemove()
        {
            var publisher = new Publisher();
            var (weakManager, _, weakSubscriber) = WeakEventHandlerCore(null, publisher, null, remove: true);
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
            int raiseCount = 1, bool remove = false)
        {
            manager ??= new Manager();
            publisher ??= new Publisher();
            subscriber ??= new Subscriber();
            Assert.AreEqual(0, publisher.EventHandlerCount);
            manager.Add(publisher, subscriber);
            Assert.AreEqual(1, publisher.EventHandlerCount);

            Assert.AreEqual(0, subscriber.HandlerCallCount);
            for (int i = 0; i < raiseCount; i++) publisher.RaiseEvent();
            Assert.AreEqual(raiseCount, subscriber.HandlerCallCount);
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

            return (new WeakReference<Manager>(manager), new WeakReference<Publisher>(publisher), new WeakReference<Subscriber>(subscriber));
        }

        private class Manager
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Publisher publisher, Subscriber subscriber) => Proxy = WeakEvent.CollectionChanged.Add(publisher, subscriber.Handler);
        }

        private class Publisher : INotifyCollectionChanged
        {
            private static readonly NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            private NotifyCollectionChangedEventHandler? collectionChanged;

            public int EventHandlerCount { get; private set; }

            public event NotifyCollectionChangedEventHandler? CollectionChanged
            {
                add
                {
                    collectionChanged += value;
                    EventHandlerCount++;
                }
                remove
                {
                    collectionChanged -= value;
                    EventHandlerCount--;
                }
            }

            public void RaiseEvent() => collectionChanged?.Invoke(this, args);
        }

        private class Subscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, NotifyCollectionChangedEventArgs e) => HandlerCallCount++;
        }
    }
}
