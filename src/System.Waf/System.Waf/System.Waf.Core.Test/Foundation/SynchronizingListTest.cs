using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class SynchronizingListTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new SynchronizingList<MyDataModel, MyModel>(null!, null!));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new SynchronizingList<MyDataModel, MyModel>(new ObservableList<MyModel>(), null!));
        }

        [TestMethod]
        public void SynchronizeTest()
        {
            var originalList = new ObservableList<MyModel>()
            {
                new MyModel(),
                new MyModel(),
                new MyModel()
            };

            var synchronizingList = new SynchronizingList<MyDataModel, MyModel>(originalList, m => new MyDataModel(m));
            AssertHelper.SequenceEqual(originalList, synchronizingList.Select(dm => dm.Model));

            // Check add operation with collection changed event.
            int handlerCalled = 0;
            NotifyCollectionChangedEventHandler handler = (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(3, e.NewStartingIndex);
                Assert.AreEqual(originalList.Last(), e.NewItems!.Cast<MyDataModel>().Single().Model);
            };
            AssertCollectionChangeEventsCalled(handler, () => originalList.Add(new MyModel()));
            AssertHelper.ExpectedException<NotSupportedException>(() => synchronizingList.Add(new MyDataModel(new MyModel())));

            // Check insert at index 0 operation with collection changed event.
            handler = (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(0, e.NewStartingIndex);
                Assert.AreEqual(originalList[0], e.NewItems!.Cast<MyDataModel>().Single().Model);
            };
            AssertCollectionChangeEventsCalled(handler, () => originalList.Insert(0, new MyModel()));
            AssertHelper.ExpectedException<NotSupportedException>(() => synchronizingList.Insert(0, new MyDataModel(new MyModel())));

            // Compare the collections
            AssertHelper.SequenceEqual(originalList, synchronizingList.Select(dm => dm.Model));

            // Check remove operation with collection changed event.
            MyModel itemToRemove = originalList[2];
            handler = (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(2, e.OldStartingIndex);
                Assert.AreEqual(itemToRemove, e.OldItems!.Cast<MyDataModel>().Single().Model);
            };
            AssertCollectionChangeEventsCalled(handler, () => originalList.Remove(itemToRemove));
            originalList.Insert(2, itemToRemove);
            AssertCollectionChangeEventsCalled(handler, () => synchronizingList.Remove(synchronizingList[2]));

            // Check replace operation with collection changed event.
            MyModel itemToReplace = originalList[1];
            handler = (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Replace, e.Action);
                Assert.AreEqual(1, e.NewStartingIndex);
                Assert.AreEqual(1, e.OldStartingIndex);
                Assert.AreEqual(originalList[1], e.NewItems!.Cast<MyDataModel>().Single().Model);
                Assert.AreEqual(itemToReplace, e.OldItems!.Cast<MyDataModel>().Single().Model);
            };
            AssertCollectionChangeEventsCalled(handler, () => originalList[1] = new MyModel());
            AssertHelper.ExpectedException<NotSupportedException>(() => synchronizingList[1] = new MyDataModel(new MyModel()));

            // Check move operation with collection changed event.
            handler = (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Move, e.Action);
                Assert.AreEqual(0, e.OldStartingIndex);
                Assert.AreEqual(2, e.NewStartingIndex);
            };
            AssertCollectionChangeEventsCalled(handler, () => originalList.Move(0, 2));
            originalList.Move(2, 0);
            AssertCollectionChangeEventsCalled(handler, () => synchronizingList.Move(0, 2));

            // Check clear operation with collection changed event.
            handler = (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
            };
            var backup = originalList.ToArray();
            AssertCollectionChangeEventsCalled(handler, () => originalList.Clear());
            foreach (var x in backup) originalList.Add(x);
            AssertCollectionChangeEventsCalled(handler, () => synchronizingList.Clear());

            Assert.IsFalse(synchronizingList.Any());

            void AssertCollectionChangeEventsCalled(NotifyCollectionChangedEventHandler handler, Action action)
            {
                handlerCalled = 0;
                synchronizingList!.CollectionChanging += OuterHandler;
                synchronizingList.CollectionChanged += OuterHandler;
                action();
                synchronizingList.CollectionChanging -= OuterHandler;
                synchronizingList.CollectionChanged -= OuterHandler;
                Assert.AreEqual(2, handlerCalled);

                void OuterHandler(object? sender, NotifyCollectionChangedEventArgs e)
                {
                    handlerCalled++;
                    Assert.AreEqual(synchronizingList, sender);
                    handler(sender, e);
                }
            }
        }

        [TestMethod]
        public void SynchronizeCustomCollectionTest()
        {
            var originalList = new CustomCollection<MyModel>
            {
                new MyModel(),
                new MyModel(),
                new MyModel()
            };

            var synchronizingList = new SynchronizingList<MyDataModel, MyModel>(originalList, m => new MyDataModel(m));
            AssertHelper.SequenceEqual(originalList, synchronizingList.Select(dm => dm.Model));

            // Check add operation with collection changed event.
            int handlerCalled = 0;
            NotifyCollectionChangedEventHandler handler = (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(3, e.NewStartingIndex);
                Assert.AreEqual(originalList.Last(), e.NewItems!.Cast<MyDataModel>().Single().Model);
            };
            AssertCollectionChangeEventsCalled(handler, () => originalList.Add(new MyModel()));

            // Compare the collections
            AssertHelper.SequenceEqual(originalList, synchronizingList.Select(dm => dm.Model));

            // Check remove operation with collection changed event.
            MyModel itemToRemove = originalList[2];
            handler = (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(2, e.OldStartingIndex);
                Assert.AreEqual(itemToRemove, e.OldItems!.Cast<MyDataModel>().Single().Model);
            };
            AssertCollectionChangeEventsCalled(handler, () => originalList.Remove(itemToRemove));

            // Check replace operation with collection changed event.
            MyModel itemToReplace = originalList[1];
            int customHandlerCalled = 0;
            handler = (sender, e) =>
            {
                Assert.AreEqual(synchronizingList, sender);
                if (customHandlerCalled == 0)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                    Assert.AreEqual(itemToReplace, e.OldItems!.Cast<MyDataModel>().Single().Model);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(originalList[1], e.NewItems!.Cast<MyDataModel>().Single().Model);
                }
                customHandlerCalled++;
            };
            synchronizingList.CollectionChanged += handler;
            originalList[1] = new MyModel();
            synchronizingList.CollectionChanged -= handler;
            Assert.AreEqual(2, customHandlerCalled);

            // Check reset operation with collection changed event.
            var newItems = new List<MyModel>()
            {
                new MyModel(),
                new MyModel()
            };
            customHandlerCalled = 0;
            handler = (sender, e) =>
            {
                Assert.AreEqual(synchronizingList, sender);
                if (customHandlerCalled == 0)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                }
                customHandlerCalled++;
            };
            synchronizingList.CollectionChanged += handler;
            originalList.Reset(newItems);
            synchronizingList.CollectionChanged -= handler;
            Assert.AreEqual(3, customHandlerCalled);
            AssertHelper.SequenceEqual(newItems, synchronizingList.Select(dm => dm.Model));

            void AssertCollectionChangeEventsCalled(NotifyCollectionChangedEventHandler handler, Action action)
            {
                handlerCalled = 0;
                synchronizingList!.CollectionChanging += OuterHandler;
                synchronizingList.CollectionChanged += OuterHandler;
                action();
                synchronizingList.CollectionChanging -= OuterHandler;
                synchronizingList.CollectionChanged -= OuterHandler;
                Assert.AreEqual(2, handlerCalled);

                void OuterHandler(object? sender, NotifyCollectionChangedEventArgs e)
                {
                    handlerCalled++;
                    Assert.AreEqual(synchronizingList, sender);
                    handler(sender, e);
                }
            }
        }

        [TestMethod]
        public void PropertyChangedTest()
        {
            var originalList = new ObservableList<MyModel>();
            var synchronizingList = new SynchronizingList<MyDataModel, MyModel>(originalList, m => new MyDataModel(m));

            // Check that the PropertyChanged event for Count is raised.
            bool handlerCalled = false;
            PropertyChangedEventHandler handler = (sender, e) =>
            {
                Assert.AreEqual(synchronizingList, sender);
                if (e.PropertyName == nameof(SynchronizingList<MyDataModel, MyModel>.Count))
                {
                    handlerCalled = true;
                }
            };
            synchronizingList.PropertyChanged += handler;
            originalList.Add(new MyModel());
            synchronizingList.PropertyChanged -= handler;
            Assert.IsTrue(handlerCalled);

            // Check that after unwiring the event the handler is not called anymore.
            handlerCalled = false;
            originalList.Add(new MyModel());
            Assert.IsFalse(handlerCalled);
        }

        [TestMethod]
        public void WeakEventTest()
        {
            var originalList = new ObservableList<MyModel>();

            // Check that no memory leak occurs
            var weakSynchronizingCollection = WeakTest(originalList);
            GC.Collect();
            Assert.IsFalse(weakSynchronizingCollection.IsAlive);

            static WeakReference WeakTest(ObservableList<MyModel> originalCollection)
            {
                var synchronizingCollection = new SynchronizingList<MyDataModel, MyModel>(originalCollection, m => new MyDataModel(m));
                var weakSynchronizingCollection = new WeakReference(synchronizingCollection);
                originalCollection.Add(new MyModel());
                Assert.IsTrue(weakSynchronizingCollection.IsAlive);
                return weakSynchronizingCollection;
            }
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

        private class CustomCollection<T> : ObservableCollection<T>
        {
            private bool suppressNotifications;

            public void Reset(IEnumerable<T> newItems)
            {
                try
                {
                    suppressNotifications = true;
                    Clear();
                    if (newItems != null)
                    {
                        foreach (var x in newItems) Add(x);
                    }
                }
                finally
                {
                    suppressNotifications = false;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }

            protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                if (suppressNotifications) return;
                e = e.Action switch
                {
                    NotifyCollectionChangedAction.Add => new NotifyCollectionChangedEventArgs(e.Action, e.NewItems),
                    NotifyCollectionChangedAction.Remove => new NotifyCollectionChangedEventArgs(e.Action, e.OldItems),
                    NotifyCollectionChangedAction.Replace => new NotifyCollectionChangedEventArgs(e.Action, e.NewItems!, e.OldItems!),
                    NotifyCollectionChangedAction.Reset => new NotifyCollectionChangedEventArgs(e.Action),
                    _ => throw new NotSupportedException(e.Action.ToString())
                };
                base.OnCollectionChanged(e);
            }
        }
    }
}
