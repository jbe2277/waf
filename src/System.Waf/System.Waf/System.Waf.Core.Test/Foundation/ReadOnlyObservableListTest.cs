using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Waf.Foundation;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class ReadOnlyObservableListTest
    {
        [TestMethod]
        public void ObservableCollectionChangedEventsTest() => ChangedEventsTestCore(new ObservableCollection<string>(), false);

        [TestMethod]
        public void ObservableListChangedEventsTest() => ChangedEventsTestCore(new ObservableList<string>(), true);

        private static void ChangedEventsTestCore(ObservableCollection<string> list, bool supportsCollectionChanging)
        {
            var readOnlyList = new ReadOnlyObservableList<string>(list);

            int collectionChangingCalled = 0;            
            int collectionChangedCalled = 0;
            int countPropertyChangedCalled = 0;
            
            if (supportsCollectionChanging) readOnlyList.CollectionChanging += CollectionChangingHandler;
            else Assert.ThrowsException<NotSupportedException>(() => readOnlyList.CollectionChanging += CollectionChangingHandler);
            readOnlyList.CollectionChanged += CollectionChangedHandler;
            readOnlyList.PropertyChanged += PropertyChangedHandler;
            list.Add("Hello");
            if (supportsCollectionChanging) Assert.AreEqual(1, collectionChangingCalled);
            Assert.AreEqual(1, collectionChangedCalled);
            Assert.AreEqual(1, countPropertyChangedCalled);

            readOnlyList.CollectionChanging -= CollectionChangingHandler;
            readOnlyList.CollectionChanged -= CollectionChangedHandler;
            readOnlyList.PropertyChanged -= PropertyChangedHandler;
            list.Add("World");
            if (supportsCollectionChanging) Assert.AreEqual(1, collectionChangingCalled);
            Assert.AreEqual(1, collectionChangedCalled);
            Assert.AreEqual(1, countPropertyChangedCalled);

            void CollectionChangingHandler(object? sender, NotifyCollectionChangedEventArgs e) { Assert.AreSame(readOnlyList, sender); collectionChangingCalled++; }
            void CollectionChangedHandler(object? sender, NotifyCollectionChangedEventArgs e) { Assert.AreSame(readOnlyList, sender); collectionChangedCalled++; }
            void PropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
            {
                Assert.AreSame(readOnlyList, sender);
                if (e.PropertyName == nameof(readOnlyList.Count)) countPropertyChangedCalled++;
            }
        }

        [TestMethod]
        public void EmptyPropertyTest()
        {
            var readOnlyList = ReadOnlyObservableList<string>.Empty;
            Assert.AreEqual(0, readOnlyList.Count);
        }
    }
}
