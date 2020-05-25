using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventPropertyChangedTest : WeakEventTestBase<WeakEventPropertyChangedTest.Manager, WeakEventPropertyChangedTest.Publisher, WeakEventPropertyChangedTest.Subscriber>
    {
        [TestMethod]
        public void WeakEventAddArgumentException()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.PropertyChanged.Add(null!, (s, h) => { }));
            AssertHelper.ExpectedException<ArgumentNullException>(() => WeakEvent.PropertyChanged.Add(new Publisher(), null!));
        }

        public class Manager : IManager<Publisher, Subscriber>
        {
            public IWeakEventProxy? Proxy { get; set; }

            public void Add(Publisher publisher, Subscriber subscriber) => Proxy = WeakEvent.PropertyChanged.Add(publisher, subscriber.Handler);
        }

        public class Publisher : IPublisher, INotifyPropertyChanged
        {
            private static readonly PropertyChangedEventArgs args = new PropertyChangedEventArgs("Test");
            private PropertyChangedEventHandler? propertyChanged;

            public int EventHandlerCount { get; private set; }

            public event PropertyChangedEventHandler? PropertyChanged
            {
                add
                {
                    propertyChanged += value;
                    EventHandlerCount++;
                }
                remove
                {
                    propertyChanged -= value;
                    EventHandlerCount--;
                }
            }

            public void RaiseEvent() => propertyChanged?.Invoke(this, args);
        }

        public class Subscriber : ISubscriber
        {
            public int HandlerCallCount { get; set; }

            public void Handler(object? sender, PropertyChangedEventArgs e) => HandlerCallCount++;
        }
    }
}
