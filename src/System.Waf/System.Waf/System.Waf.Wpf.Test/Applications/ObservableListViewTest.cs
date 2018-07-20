using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Foundation;

namespace Test.Waf.Applications
{
    [TestClass]
    public class ObservableListViewTest
    {
        [TestMethod]
        public void CollectionChangedTest()
        {
            var originalCollection = new ObservableCollection<MyModel>() { new MyModel() };
            var listView = new ObservableListView<MyModel>(originalCollection);
            Assert.IsTrue(originalCollection.SequenceEqual(listView));

            // Check add operation with collection changed event.
            bool handlerCalled = false;
            NotifyCollectionChangedEventHandler handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(listView, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewStartingIndex);
                Assert.AreEqual(originalCollection.Last(), e.NewItems.Cast<MyModel>().Single());
            };
            listView.CollectionChanged += handler;
            originalCollection.Add(new MyModel());
            Assert.IsTrue(handlerCalled);

            // After dispose the collection does not synchronize anymore
            handlerCalled = false;
            listView.Dispose();
            originalCollection.Add(new MyModel());
            Assert.IsFalse(handlerCalled);
            listView.CollectionChanged -= handler;
        }

        [TestMethod]
        public void DisposeTest()
        {
            var originalCollection = new ObservableCollection<MyModel>();
            var filterCalled = false;
            var listView = new ObservableListView<MyModel>(originalCollection, null, x =>
            {
                filterCalled = true;
                return true;
            });
            originalCollection.Add(new MyModel());
            Assert.IsTrue(filterCalled);

            // Calling dispose twice must not throw an exception.
            listView.Dispose();
            listView.Dispose();
            filterCalled = false;
            originalCollection.Add(new MyModel());
            Assert.IsFalse(filterCalled);
        }

        [TestMethod]
        public void WeakEventHandlerTest()
        {
            var originalCollection = new ObservableCollection<MyModel>();
            var listView = new ObservableListView<MyModel>(originalCollection);
            var weakListView = new WeakReference(listView);

            originalCollection.Add(new MyModel());
            Assert.IsTrue(weakListView.IsAlive);

            listView = null;
            GC.Collect();
            Assert.IsNotNull(originalCollection);
            Assert.IsFalse(weakListView.IsAlive);
        }


        private class MyModel : Model { }
    }
}
