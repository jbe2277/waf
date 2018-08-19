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
    public class SynchronizingCollectionTest
    {
        [TestMethod]
        public void CollectionChangedTest()
        {
            var originalCollection = new ObservableCollection<MyModel>() { new MyModel() };
            var synchronizingCollection = new SynchronizingCollection<MyDataModel, MyModel>(originalCollection, m => new MyDataModel(m));
            CollectionAssert.AreEqual(originalCollection, synchronizingCollection.Select(dm => dm.Model).ToArray());

            // Check add operation with collection changed event.
            bool handlerCalled = false;
            NotifyCollectionChangedEventHandler handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(synchronizingCollection, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewStartingIndex);
                Assert.AreEqual(originalCollection.Last(), e.NewItems.Cast<MyDataModel>().Single().Model);
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection.Add(new MyModel());
            Assert.IsTrue(handlerCalled);

            // After dispose the collection does not synchronize anymore
            handlerCalled = false;
            synchronizingCollection.Dispose();
            originalCollection.Add(new MyModel());
            Assert.IsFalse(handlerCalled);
            synchronizingCollection.CollectionChanged -= handler;
        }

        [TestMethod]
        public void DisposeTest()
        {
            var originalCollection = new ObservableCollection<MyModel>();
            bool factoryCalled = false;
            var synchronizingCollection = new SynchronizingCollection<MyDataModel, MyModel>(originalCollection, m =>
            {
                factoryCalled = true;
                return new MyDataModel(m);
            });
            originalCollection.Add(new MyModel());
            Assert.IsTrue(factoryCalled);

            // Calling dispose twice must not throw an exception.
            synchronizingCollection.Dispose();
            synchronizingCollection.Dispose();
            factoryCalled = false;
            originalCollection.Add(new MyModel());
            Assert.IsFalse(factoryCalled);
        }

        [TestMethod]
        public void WeakEventHandlerTest()
        {
            var originalCollection = new ObservableCollection<MyModel>();
            var synchronizingCollection = new SynchronizingCollection<MyDataModel, MyModel>(originalCollection, m => new MyDataModel(m));
            var weakSynchronizingCollection = new WeakReference(synchronizingCollection);

            originalCollection.Add(new MyModel());
            Assert.IsTrue(weakSynchronizingCollection.IsAlive);

            synchronizingCollection = null;
            GC.Collect();
            Assert.IsNotNull(originalCollection);
            Assert.IsFalse(weakSynchronizingCollection.IsAlive);
        }



        private class MyDataModel : Model
        {
            public MyDataModel(MyModel model)
            {
                Model = model;
            }


            public MyModel Model { get; }
        }

        private class MyModel : Model { }
    }
}
