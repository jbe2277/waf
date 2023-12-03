using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventCollectionChangingTest : WeakEventTestBase<WeakEventCollectionChangingTest.Manager, WeakEventCollectionChangingTest.Publisher, WeakEventCollectionChangingTest.Subscriber>
    {
        [TestMethod]
        public void WeakEventAddArgumentException()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.CollectionChanging.Add(null!, (s, h) => { }));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.CollectionChanging.Add(new Publisher(), null!));
        }

        public class Manager : IManager<Publisher, Subscriber>
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Publisher publisher, Subscriber subscriber) => Proxy = WeakEvent.CollectionChanging.Add(publisher, subscriber.Handler);
        }

        public class Publisher : IPublisher, INotifyCollectionChanging
        {
            private static readonly NotifyCollectionChangedEventArgs args = new(NotifyCollectionChangedAction.Reset);
            private NotifyCollectionChangedEventHandler? collectionChanging;

            public int EventHandlerCount { get; private set; }

            public event NotifyCollectionChangedEventHandler? CollectionChanging
            {
                add
                {
                    collectionChanging += value;
                    EventHandlerCount++;
                }
                remove
                {
                    collectionChanging -= value;
                    EventHandlerCount--;
                }
            }

            public void RaiseEvent() => collectionChanging?.Invoke(this, args);
        }

        public class Subscriber : ISubscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, NotifyCollectionChangedEventArgs e) => HandlerCallCount++;
        }
    }
}
