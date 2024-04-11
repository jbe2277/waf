using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    internal class CollectionEventsTestModel : Model
    {
        private string? name;

        public string? Name { get => name; set => SetProperty(ref name, value); }
    }


    [TestClass]
    public class ObservableListTest
    {
        [TestMethod]
        public void AddSameObjectTwiceTest()
        {
            var collectionItemChangedList = new List<(object? item, string? propertyName)>();

            var item = new CollectionEventsTestModel { Name = "1" };
            var list = new ObservableList<CollectionEventsTestModel?>(new[] { item, item });
            list.CollectionItemChanged += CollectionItemChangedHandler;
            list.Add(item);
            list.Add(null);
            list[list.Count - 1] = item;
            item.Name = "2";
            Assert.AreEqual(nameof(item.Name), collectionItemChangedList.Single().propertyName);
            collectionItemChangedList.Clear();

            list[list.Count - 1] = null;
            list.Remove(item);
            item.Name = "3";
            Assert.AreEqual(nameof(item.Name), collectionItemChangedList.Single().propertyName);
            collectionItemChangedList.Clear();

            list.Clear();
            item.Name = "4";
            Assert.AreEqual(0, collectionItemChangedList.Count);

            void CollectionItemChangedHandler(object? item, PropertyChangedEventArgs e) => collectionItemChangedList.Add((item, e.PropertyName));
        }

        [TestMethod]
        public void UseNullAsItemTest()
        {
            var list = new ObservableList<CollectionEventsTestModel?>(new CollectionEventsTestModel?[] { null });
            AssertHelper.SequenceEqual(new CollectionEventsTestModel?[] { null }, list);
            list.Remove(null);
            Assert.AreEqual(0, list.Count);

            list.Add(null);
            AssertHelper.SequenceEqual(new CollectionEventsTestModel?[] { null }, list);
            list.RemoveAt(0);
            Assert.AreEqual(0, list.Count);

            list.Add(new());
            list[0] = null;
            AssertHelper.SequenceEqual(new CollectionEventsTestModel?[] { null }, list);
            list.Clear();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void PropertyChangedEventTest()
        {
            var list = new ObservableList<CollectionEventsTestModel>(new[] { new CollectionEventsTestModel { Name = "first" } });

            bool countChangedHandlerCalled = false;
            list.PropertyChanged += PropertyChangedHandler;
            list.Add(new() { Name = "second" });
            Assert.IsTrue(countChangedHandlerCalled);

            countChangedHandlerCalled = false;
            list.PropertyChanged -= PropertyChangedHandler;
            list.Add(new() { Name = "third" });
            Assert.IsFalse(countChangedHandlerCalled);

            void PropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
            {
                Assert.AreEqual(list, sender);
                if (e.PropertyName == nameof(list.Count)) countChangedHandlerCalled = true;
            }
        }

        [TestMethod]
        public void CollectionEventsTest()
        {
            var list = new ObservableList<CollectionEventsTestModel>();
            CollectionEventsTestCore(list, list);
        }

        internal static void CollectionEventsTestCore(ObservableCollection<CollectionEventsTestModel> source, object observable)
        {
            var collectionChangingArgs = new List<NotifyCollectionChangedEventArgs>();
            var collectionChangedArgs = new List<NotifyCollectionChangedEventArgs>();

            ((INotifyCollectionChanging)observable).CollectionChanging += CollectionChangingHandler;
            ((INotifyCollectionChanged)observable).CollectionChanged += CollectionChangedHandler;
            source.Add(new() { Name = "first" });
            AssertLastListEvent();

            source.Insert(1, new() { Name = "second" });
            AssertLastListEvent();

            source.Insert(1, new() { Name = "third" });
            AssertLastListEvent();

            source.Move(2, 0);
            AssertLastListEvent();

            source[1] = new() { Name = "fourth" };
            AssertLastListEvent();

            source.Remove(source[0]);
            AssertLastListEvent();

            source.Clear();
            AssertLastListEvent();

            Assert.AreEqual(7, collectionChangingArgs.Count);

            void CollectionChangingHandler(object? sender, NotifyCollectionChangedEventArgs e)
            {
                Assert.AreEqual(collectionChangedArgs.Count, collectionChangingArgs.Count);
                Assert.AreEqual(observable, sender);
                collectionChangingArgs.Add(e);
            }

            void CollectionChangedHandler(object? sender, NotifyCollectionChangedEventArgs e)
            {
                Assert.AreEqual(observable, sender);
                collectionChangedArgs.Add(e);
                Assert.AreEqual(collectionChangedArgs.Count, collectionChangingArgs.Count);
            }

            void AssertLastListEvent() => AssertEqualEventArgs(collectionChangedArgs.Last(), collectionChangingArgs.Last());

            static void AssertEqualEventArgs(NotifyCollectionChangedEventArgs expected, NotifyCollectionChangedEventArgs actual)
            {
                Assert.AreEqual(expected.Action, actual.Action);
                Assert.AreEqual(expected.NewStartingIndex, actual.NewStartingIndex);
                Assert.AreEqual(expected.OldStartingIndex, actual.OldStartingIndex);
                AssertHelper.SequenceEqual(ToGeneric(expected.NewItems), ToGeneric(actual.NewItems));
                AssertHelper.SequenceEqual(ToGeneric(expected.OldItems), ToGeneric(actual.OldItems));

                static IEnumerable<object> ToGeneric(IList? list) => list?.OfType<object>() ?? Array.Empty<object>();
            }
        }

        [TestMethod]
        public void CollectionItemChangedTest()
        {
            var list = new ObservableList<CollectionEventsTestModel>(new[] { new CollectionEventsTestModel() });
            CollectionItemChangedTestCore(list, list);
        }

        internal static void CollectionItemChangedTestCore(ObservableCollection<CollectionEventsTestModel> source, object observable)
        {
            var collectionItemChangedList = new List<(object? item, string? propertyName)>();

            var item1 = source[0];
            item1.Name = "Empty";

            ((INotifyCollectionItemChanged)observable).CollectionItemChanged += CollectionItemChangedHandler;

            item1.Name = "First";
            Assert.AreEqual((item1, nameof(CollectionEventsTestModel.Name)), collectionItemChangedList.Last());
            Assert.AreEqual(1, collectionItemChangedList.Count);

            var item2 = new CollectionEventsTestModel();
            source.Add(item2);
            item2.Name = "Second";
            Assert.AreEqual((item2, nameof(CollectionEventsTestModel.Name)), collectionItemChangedList.Last());
            Assert.AreEqual(2, collectionItemChangedList.Count);

            var item2b = new CollectionEventsTestModel();
            source[1] = item2b;
            item2b.Name = "Second B";
            Assert.AreEqual((item2b, nameof(CollectionEventsTestModel.Name)), collectionItemChangedList.Last());
            Assert.AreEqual(3, collectionItemChangedList.Count);

            item2.Name = "Removed 2";
            Assert.AreEqual(3, collectionItemChangedList.Count);

            source.Remove(item2b);
            item2b.Name = "Removed 2B";
            Assert.AreEqual(3, collectionItemChangedList.Count);

            source.Clear();
            item1.Name = "Cleared 1";
            Assert.AreEqual(3, collectionItemChangedList.Count);

            void CollectionItemChangedHandler(object? item, PropertyChangedEventArgs e) => collectionItemChangedList.Add((item, e.PropertyName));
        }

        [TestMethod]
        public void CollectionItemChangedWeakTest()
        {
            CollectionItemChangedWeakCoreTest(() => new ObservableList<CollectionEventsTestModel>(new[] { new CollectionEventsTestModel() }));
        }

        internal static void CollectionItemChangedWeakCoreTest(Func<IList<CollectionEventsTestModel>> factory)
        {
            var (weakList, item1) = Core();
            GC.Collect();
            Assert.IsNotNull(item1);
            Assert.IsFalse(weakList.TryGetTarget(out _));

            (WeakReference<IList<CollectionEventsTestModel>> weakList, CollectionEventsTestModel item1) Core()
            {
                var list = factory();
                return (new(list), list[0]);
            }
        }

        [TestMethod]
        public void CollectionItemChangedSpecialTest()
        {
            var list1 = new ObservableList<int> { 1 };
            list1.Clear();
            list1.Add(2);
            AssertHelper.SequenceEqual(new[] { 2 }, list1);

            var list2 = new ObservableList<object> { new object() };
            list2.Clear();
            list2.Add(new object());
            Assert.AreEqual(1, list2.Count);
        }
    }
}
