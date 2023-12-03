using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventCollectionItemChangedTest : WeakEventTestBase<WeakEventCollectionItemChangedTest.Manager, WeakEventCollectionItemChangedTest.Publisher, WeakEventCollectionItemChangedTest.Subscriber>
    {
        [TestMethod]
        public void WeakEventAddArgumentException()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.CollectionItemChanged.Add(null!, (s, h) => { }));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.CollectionItemChanged.Add(new Publisher(), null!));
        }

        public class Manager : IManager<Publisher, Subscriber>
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Publisher publisher, Subscriber subscriber) => Proxy = WeakEvent.CollectionItemChanged.Add(publisher, subscriber.Handler);
        }

        public class Publisher : IPublisher, INotifyCollectionItemChanged
        {
            private static readonly PropertyChangedEventArgs args = new("Test");
            private PropertyChangedEventHandler? collectionItemChanged;

            public int EventHandlerCount { get; private set; }

            public event PropertyChangedEventHandler? CollectionItemChanged
            {
                add
                {
                    collectionItemChanged += value;
                    EventHandlerCount++;
                }
                remove
                {
                    collectionItemChanged -= value;
                    EventHandlerCount--;
                }
            }

            public void RaiseEvent() => collectionItemChanged?.Invoke(this, args);
        }

        public class Subscriber : ISubscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, PropertyChangedEventArgs e) => HandlerCallCount++;
        }
    }
}
