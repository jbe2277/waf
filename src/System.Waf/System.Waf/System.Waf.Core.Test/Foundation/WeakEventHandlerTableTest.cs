using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Waf.Foundation;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class WeakEventHandlerTableTest
    {
        [TestMethod]
        public void AddToTable()
        {
            var table = new WeakEventHandlerTable();
            var weakSubscriber = Core();
            GC.Collect();
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));

            WeakReference<Subscriber> Core()
            {
                var subscriber = new Subscriber();
                table.Add((PropertyChangedEventHandler)subscriber.Handler1);
                return new(subscriber);
            }
        }

        [TestMethod]
        public void AddAndRemove1()
        {
            var table = new WeakEventHandlerTable();
            var weakSubscriber = Core();
            GC.Collect();
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));

            WeakReference<Subscriber> Core()
            {
                var subscriber = new Subscriber();
                var h1 = (PropertyChangedEventHandler)subscriber.Handler1;
                table.Add(h1);
                table.Remove(h1);
                return new(subscriber);
            }
        }

        [TestMethod]
        public void AddAndRemove2()
        {
            var table = new WeakEventHandlerTable();
            var weakSubscriber = Core();
            GC.Collect();
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _));

            WeakReference<Subscriber> Core()
            {
                var subscriber = new Subscriber();
                var h1 = (PropertyChangedEventHandler)subscriber.Handler1;
                var h2 = (PropertyChangedEventHandler)subscriber.Handler2;
                var sh = (PropertyChangedEventHandler)Subscriber.StaticHandler;
                table.Add(h1);
                table.Add(h2);
                table.Add(sh);
                table.Remove(h1);
                table.Remove(h2);
                table.Remove(sh);
                return new(subscriber);
            }
        }

        [TestMethod]
        public void WeakEventHandlerAliveTest()
        {
            var publisher = new Publisher();
            var subscriber = new Subscriber();
            var proxy1 = Add1();
            GC.Collect();
            Assert.AreEqual(0, subscriber.Handler1CallCount);
            publisher.Name = "Test1";
            Assert.AreEqual(1, subscriber.Handler1CallCount);
            GC.Collect();
            publisher.Name = "Test2";
            Assert.AreEqual(2, subscriber.Handler1CallCount);

            var proxy2 = Add2();
            var staticProxy = StaticAdd();
            GC.Collect();
            Assert.AreEqual(0, subscriber.Handler2CallCount);
            publisher.Name = "Test3";
            Assert.AreEqual(3, subscriber.Handler1CallCount);
            Assert.AreEqual(1, subscriber.Handler2CallCount);
            GC.Collect();
            publisher.Name = "Test4";
            Assert.AreEqual(4, subscriber.Handler1CallCount);
            Assert.AreEqual(2, subscriber.Handler2CallCount);

            proxy1.Remove();
            proxy2.Remove();
            staticProxy.Remove();
            publisher.Name = "Test5";
            Assert.AreEqual(4, subscriber.Handler1CallCount);
            Assert.AreEqual(2, subscriber.Handler2CallCount);

            IWeakEventProxy Add1() => WeakEvent.PropertyChanged.Add(publisher, subscriber.Handler1);
            IWeakEventProxy Add2() => WeakEvent.PropertyChanged.Add(publisher, subscriber.Handler2);
            IWeakEventProxy StaticAdd() => WeakEvent.PropertyChanged.Add(publisher, Subscriber.StaticHandler);
        }

        private class Publisher : Model
        {
            private string? name;

            public string? Name
            {
                get => name;
                set => SetProperty(ref name, value);
            }
        }

        private class Subscriber
        {
            public int Handler1CallCount { get; set; }
            public int Handler2CallCount { get; set; }
            public static int StaticHandlerCallCount { get; set; }

            public void Handler1(object? sender, PropertyChangedEventArgs e) => Handler1CallCount++;

            public void Handler2(object? sender, PropertyChangedEventArgs e) => Handler2CallCount++;

            public static void StaticHandler(object? sender, PropertyChangedEventArgs e) => StaticHandlerCallCount++;
        }
    }
}
