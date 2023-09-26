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
    [TestClass, Obsolete]
    public class SynchronizingCollectionCoreTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new SynchronizingCollectionCore<MyDataModel, MyModel>(
                null!, null!));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new SynchronizingCollectionCore<MyDataModel, MyModel>(
                new List<MyModel>(), null!));
        }

        [TestMethod]
        public void ObservableCollectionTest()
        {
            var originalCollection = new ObservableCollection<MyModel>()
            {
                new MyModel(),
                new MyModel(),
                new MyModel()
            };

            using var synchronizingCollection = new SynchronizingCollectionCore<MyDataModel, MyModel>(originalCollection, m => new MyDataModel(m));
            AssertHelper.SequenceEqual(originalCollection, synchronizingCollection.Select(dm => dm.Model));

            // Check add operation with collection changed event.
            bool handlerCalled = false;
            NotifyCollectionChangedEventHandler handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(synchronizingCollection, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(3, e.NewStartingIndex);
                Assert.AreEqual(originalCollection.Last(), e.NewItems!.Cast<MyDataModel>().Single().Model);
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection.Add(new MyModel());
            synchronizingCollection.CollectionChanged -= handler;
            Assert.IsTrue(handlerCalled);

            // Check insert at index 0 operation with collection changed event.
            handlerCalled = false;
            handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(synchronizingCollection, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(0, e.NewStartingIndex);
                Assert.AreEqual(originalCollection[0], e.NewItems!.Cast<MyDataModel>().Single().Model);
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection.Insert(0, new MyModel());
            synchronizingCollection.CollectionChanged -= handler;
            Assert.IsTrue(handlerCalled);

            // Compare the collections
            AssertHelper.SequenceEqual(originalCollection, synchronizingCollection.Select(dm => dm.Model));

            // Check remove operation with collection changed event.
            MyModel itemToRemove = originalCollection[2];
            handlerCalled = false;
            handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(synchronizingCollection, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(2, e.OldStartingIndex);
                Assert.AreEqual(itemToRemove, e.OldItems!.Cast<MyDataModel>().Single().Model);
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection.Remove(itemToRemove);
            synchronizingCollection.CollectionChanged -= handler;
            Assert.IsTrue(handlerCalled);

            // Check replace operation with collection changed event.
            MyModel itemToReplace = originalCollection[1];
            handlerCalled = false;
            handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(synchronizingCollection, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Replace, e.Action);
                Assert.AreEqual(1, e.NewStartingIndex);
                Assert.AreEqual(1, e.OldStartingIndex);
                Assert.AreEqual(originalCollection[1], e.NewItems!.Cast<MyDataModel>().Single().Model);
                Assert.AreEqual(itemToReplace, e.OldItems!.Cast<MyDataModel>().Single().Model);
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection[1] = new MyModel();
            synchronizingCollection.CollectionChanged -= handler;
            Assert.IsTrue(handlerCalled);

            // Check move operation with collection changed event.
            handlerCalled = false;
            handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(synchronizingCollection, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Move, e.Action);
                Assert.AreEqual(0, e.OldStartingIndex);
                Assert.AreEqual(2, e.NewStartingIndex);
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection.Move(0, 2);
            synchronizingCollection.CollectionChanged -= handler;
            Assert.IsTrue(handlerCalled);

            // Check clear operation with collection changed event.
            handlerCalled = false;
            handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(synchronizingCollection, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection.Clear();
            synchronizingCollection.CollectionChanged -= handler;
            Assert.IsTrue(handlerCalled);
            Assert.IsFalse(synchronizingCollection.Any());
        }

        [TestMethod]
        public void CustomCollectionTest()
        {
            var originalCollection = new CustomCollection<MyModel>(true)
            {
                new MyModel(),
                new MyModel(),
                new MyModel()
            };

            using var synchronizingCollection = new SynchronizingCollectionCore<MyDataModel, MyModel>(
                originalCollection, m => new MyDataModel(m));
            AssertHelper.SequenceEqual(originalCollection, synchronizingCollection.Select(dm => dm.Model));

            // Check add operation with collection changed event.
            bool handlerCalled = false;
            NotifyCollectionChangedEventHandler handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(synchronizingCollection, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(3, e.NewStartingIndex);
                Assert.AreEqual(originalCollection.Last(), e.NewItems!.Cast<MyDataModel>().Single().Model);
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection.Add(new MyModel());
            synchronizingCollection.CollectionChanged -= handler;
            Assert.IsTrue(handlerCalled);

            // Compare the collections
            AssertHelper.SequenceEqual(originalCollection, synchronizingCollection.Select(dm => dm.Model));

            // Check remove operation with collection changed event.
            MyModel itemToRemove = originalCollection[2];
            handlerCalled = false;
            handler = (sender, e) =>
            {
                handlerCalled = true;
                Assert.AreEqual(synchronizingCollection, sender);
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(2, e.OldStartingIndex);
                Assert.AreEqual(itemToRemove, e.OldItems!.Cast<MyDataModel>().Single().Model);
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection.Remove(itemToRemove);
            synchronizingCollection.CollectionChanged -= handler;
            Assert.IsTrue(handlerCalled);

            // Check replace operation with collection changed event.
            MyModel itemToReplace = originalCollection[1];
            int handlerCalledCount = 0;
            handler = (sender, e) =>
            {
                Assert.AreEqual(synchronizingCollection, sender);
                if (handlerCalledCount == 0)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                    Assert.AreEqual(itemToReplace, e.OldItems!.Cast<MyDataModel>().Single().Model);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(originalCollection[1], e.NewItems!.Cast<MyDataModel>().Single().Model);
                }
                handlerCalledCount++;
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection[1] = new MyModel();
            synchronizingCollection.CollectionChanged -= handler;
            Assert.AreEqual(2, handlerCalledCount);

            // Check reset operation with collection changed event.
            var newItems = new List<MyModel>()
            {
                new MyModel(),
                new MyModel()
            };
            handlerCalledCount = 0;
            handler = (sender, e) =>
            {
                Assert.AreEqual(synchronizingCollection, sender);
                if (handlerCalledCount == 0)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                }
                handlerCalledCount++;
            };
            synchronizingCollection.CollectionChanged += handler;
            originalCollection.Reset(newItems);
            synchronizingCollection.CollectionChanged -= handler;
            Assert.AreEqual(3, handlerCalledCount);
            AssertHelper.SequenceEqual(newItems, synchronizingCollection.Select(dm => dm.Model));
        }

        [TestMethod]
        public void PropertyChangedTest()
        {
            var originalCollection = new ObservableCollection<MyModel>();
            using var synchronizingCollection = new SynchronizingCollectionCore<MyDataModel, MyModel>(originalCollection, m => new MyDataModel(m));

            // Check that the PropertyChanged event for Count is raised.
            bool handlerCalled = false;
            PropertyChangedEventHandler handler = (sender, e) =>
            {
                Assert.AreEqual(synchronizingCollection, sender);
                if (e.PropertyName == nameof(SynchronizingCollectionCore<MyDataModel, MyModel>.Count))
                {
                    handlerCalled = true;
                }
            };
            synchronizingCollection.PropertyChanged += handler;
            originalCollection.Add(new MyModel());
            synchronizingCollection.PropertyChanged -= handler;
            Assert.IsTrue(handlerCalled);

            // Check that after unwiring the event the handler is not called anymore.
            handlerCalled = false;
            originalCollection.Add(new MyModel());
            Assert.IsFalse(handlerCalled);
        }

        [TestMethod]
        public void DisposeTest()
        {
            // Calling dispose twice must not throw an exception.
            var originalCollection = new ObservableCollection<MyModel>();
            bool factoryCalled = false;
            var synchronizingCollection = new SynchronizingCollectionCore<MyDataModel, MyModel>(originalCollection, m =>
            {
                factoryCalled = true;
                return new MyDataModel(m);
            });
            originalCollection.Add(new MyModel());
            Assert.IsTrue(factoryCalled);

            synchronizingCollection.Dispose();
            synchronizingCollection.Dispose();
            factoryCalled = false;
            originalCollection.Add(new MyModel());
            Assert.IsFalse(factoryCalled);

            // Check that no memory leak occurs
            var weakSynchronizingCollection = WeakTest(originalCollection);
            GC.Collect();
            Assert.IsFalse(weakSynchronizingCollection.IsAlive);

            static WeakReference WeakTest(ObservableCollection<MyModel> originalCollection)
            {
                var synchronizingCollection = new SynchronizingCollectionCore<MyDataModel, MyModel>(originalCollection, m => new MyDataModel(m));
                var weakSynchronizingCollection = new WeakReference(synchronizingCollection);
                originalCollection.Add(new MyModel());
                Assert.IsTrue(weakSynchronizingCollection.IsAlive);
                synchronizingCollection.Dispose();
                return weakSynchronizingCollection;
            }
        }

        [TestMethod]
        public void SubClassSynchronizingCollectionCore()
        {
            var originalCollection = new List<MyModel>();
            var synchronizingCollection = new SubSynchronizingCollection<MyDataModel, MyModel>(originalCollection, m => new MyDataModel(m));

            originalCollection.Add(new MyModel());
            Assert.AreEqual(0, synchronizingCollection.Count);
            synchronizingCollection.NotifyOriginalCollectionChanged(originalCollection, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { originalCollection.Single() }));
            Assert.AreEqual(originalCollection.Single(), synchronizingCollection.Single().Model);

            AssertHelper.ExpectedException<ArgumentNullException>(() => synchronizingCollection.NotifyOriginalCollectionChanged(originalCollection, null!));

            synchronizingCollection.Dispose();
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
            private readonly bool withoutIndex;
            private bool suppressNotifications;

            public CustomCollection(bool withoutIndex)
            {
                this.withoutIndex = withoutIndex;
            }

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
                if (withoutIndex)
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            e = new NotifyCollectionChangedEventArgs(e.Action, e.NewItems);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            e = new NotifyCollectionChangedEventArgs(e.Action, e.OldItems);
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            e = new NotifyCollectionChangedEventArgs(e.Action, e.NewItems!, e.OldItems!);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            e = new NotifyCollectionChangedEventArgs(e.Action);
                            break;
                    }
                }
                base.OnCollectionChanged(e);
            }
        }

        private class SubSynchronizingCollection<T, TOriginal> : SynchronizingCollectionCore<T, TOriginal>
        {
            public SubSynchronizingCollection(IEnumerable<TOriginal> originalCollection, Func<TOriginal, T> factory) : base(originalCollection, factory, true)
            {
            }

            public void NotifyOriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => OriginalCollectionChanged(sender, e);
        }
    }
}
