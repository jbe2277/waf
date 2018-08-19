using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Applications
{
    [TestClass]
    public class ObservableListViewTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var originalList = new ObservableCollection<string>(new[] { "D", "A", "c", "b" });
            Predicate<string> filter = x => x != "c";
            Func<IEnumerable<string>, IOrderedEnumerable<string>> sort = x => x.OrderBy(y => y);
            var listView = new ObservableListView<string>(originalList, StringComparer.OrdinalIgnoreCase);
            listView.Filter = filter;
            listView.Sort = sort;
            AssertHelper.SequenceEqual(new[] { "A", "b", "D" }, listView);
            listView.Dispose();

            listView = new ObservableListView<string>(originalList, StringComparer.OrdinalIgnoreCase, filter, sort);
            AssertHelper.SequenceEqual(new[] { "A", "b", "D" }, listView);
            Assert.AreSame(filter, listView.Filter);
            Assert.AreSame(sort, listView.Sort);
            listView.Dispose();

            AssertHelper.ExpectedException<ArgumentNullException>(() => new ObservableListView<string>(null));
        }

        [TestMethod]
        public void CollectionChangedTest()
        {
            var originalCollection = new ObservableCollection<MyModel>() { new MyModel() };
            var listView = new ObservableListView<MyModel>(originalCollection);
            AssertHelper.SequenceEqual(originalCollection, listView);

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
            }, null);
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
