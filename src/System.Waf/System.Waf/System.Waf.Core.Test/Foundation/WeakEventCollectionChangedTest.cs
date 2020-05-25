using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventCollectionChangedTest : WeakEventTestBase<WeakEventCollectionChangedTest.Manager, WeakEventCollectionChangedTest.Publisher, WeakEventCollectionChangedTest.Subscriber>
    {
        [TestMethod]
        public void WeakEventAddArgumentException()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.CollectionChanged.Add(null!, (s, h) => { }));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.CollectionChanged.Add(new Publisher(), null!));
        }

        public class Manager : IManager<Publisher, Subscriber>
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Publisher publisher, Subscriber subscriber) => Proxy = WeakEvent.CollectionChanged.Add(publisher, subscriber.Handler);
        }

        public class Publisher : IPublisher, INotifyCollectionChanged
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

        public class Subscriber : ISubscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, NotifyCollectionChangedEventArgs e) => HandlerCallCount++;
        }
    }
}
