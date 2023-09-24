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
    }
}
