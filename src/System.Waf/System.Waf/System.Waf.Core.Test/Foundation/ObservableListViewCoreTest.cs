using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Waf.Foundation;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class ObservableListViewCoreTest
    {
        private ObservableCollection<string> originalList;
        private ObservableListViewCore<string> observableListView;
        private List<NotifyCollectionChangedEventArgs> eventArgsList;
        private int countChangedCount;
        private int indexerChangedCount;

        [TestInitialize]
        public void Initialize()
        {
            originalList = new ObservableCollection<string>();
            observableListView = new ObservableListViewCore<string>(originalList);
            eventArgsList = new List<NotifyCollectionChangedEventArgs>();

            NotifyCollectionChangedEventHandler collectionHandler = (sender, e) =>
            {
                eventArgsList.Add(e);
            };
            observableListView.CollectionChanged += collectionHandler;

            PropertyChangedEventHandler propertyHandler = (sender, e) =>
            {
                if (e.PropertyName == nameof(observableListView.Count))
                {
                    countChangedCount++;
                }
                else if (e.PropertyName == "Item[]")
                {
                    indexerChangedCount++;
                }
            };
            observableListView.PropertyChanged += propertyHandler;
        }

        [TestCleanup]
        public void Cleanup()
        {
            AssertNoEventsRaised(() =>
            {
                observableListView.Dispose();
                originalList.Add("disposed");
            });
        }

        [TestMethod]
        public void RelayEventsWithoutFilter()
        {
            originalList.Add("first");
            AssertElementAdded("first", 0, eventArgsList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            eventArgsList.Clear();
            originalList.Add("second");
            AssertElementAdded("second", 1, eventArgsList.Single());

            eventArgsList.Clear();
            originalList.Add("third");
            AssertElementAdded("third", 2, eventArgsList.Single());

            eventArgsList.Clear();
            originalList.Add("four");
            AssertElementAdded("four", 3, eventArgsList.Single());

            AssertNoEventsRaised(() => observableListView.Update());  // Collection has not changed.

            eventArgsList.Clear();
            originalList.Move(0, 3);
            AssertElementMoved("first", 0, 3, eventArgsList.Single());
            eventArgsList.Clear();
            originalList.Move(3, 0);
            AssertElementMoved("first", 3, 0, eventArgsList.Single());

            eventArgsList.Clear();
            originalList.Move(3, 1);
            AssertElementMoved("four", 3, 1, eventArgsList.Single());
            eventArgsList.Clear();
            originalList.Move(1, 3);
            AssertElementMoved("four", 1, 3, eventArgsList.Single());

            eventArgsList.Clear();
            originalList.Move(2, 3);
            AssertElementMoved("third", 2, 3, eventArgsList.Single());
            eventArgsList.Clear();
            originalList.Move(3, 2);
            AssertElementMoved("four", 2, 3, eventArgsList.Single());

            eventArgsList.Clear();
            countChangedCount = indexerChangedCount = 0;
            originalList.Clear();
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, eventArgsList.Single().Action);
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            eventArgsList.Clear();
            originalList.Insert(0, "1");
            AssertElementAdded("1", 0, eventArgsList.Single());

            eventArgsList.Clear();
            originalList.Insert(1, "3");
            AssertElementAdded("3", 1, eventArgsList.Single());

            eventArgsList.Clear();
            originalList.Insert(1, "2");
            AssertElementAdded("2", 1, eventArgsList.Single());

            eventArgsList.Clear();
            countChangedCount = indexerChangedCount = 0;
            originalList.Remove("1");
            AssertElementRemoved("1", 0, eventArgsList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            eventArgsList.Clear();
            originalList.RemoveAt(1);
            AssertElementRemoved("3", 1, eventArgsList.Single());
        }

        [TestMethod]
        public void RelayEventsWithFilter()
        {
            observableListView.Filter = item => item != "second" && item != "2";

            eventArgsList.Clear();
            originalList.Add("first");
            AssertElementAdded("first", 0, eventArgsList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            AssertNoEventsRaised(() => originalList.Add("second"));

            eventArgsList.Clear();
            originalList.Add("third");
            AssertElementAdded("third", 1, eventArgsList.Single());

            AssertNoEventsRaised(() => observableListView.Update());  // Collection has not changed.

            Assert.IsTrue(new[] { "first", "third" }.SequenceEqual(observableListView));

            eventArgsList.Clear();
            countChangedCount = indexerChangedCount = 0;
            originalList.Clear();
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, eventArgsList.Single().Action);
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            eventArgsList.Clear();
            originalList.Insert(0, "1");
            AssertElementAdded("1", 0, eventArgsList.Single());

            eventArgsList.Clear();
            originalList.Insert(1, "3");
            AssertElementAdded("3", 1, eventArgsList.Single());

            AssertNoEventsRaised(() => originalList.Insert(1, "2"));

            Assert.IsTrue(new[] { "1", "3" }.SequenceEqual(observableListView));

            eventArgsList.Clear();
            countChangedCount = indexerChangedCount = 0;
            originalList.Remove("1");
            AssertElementRemoved("1", 0, eventArgsList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            eventArgsList.Clear();
            originalList.RemoveAt(1);
            AssertElementRemoved("3", 0, eventArgsList.Single());  // Index 0 because "2" is hidden by filter
        }

        private static void AssertElementAdded<T>(T newItem, int newStartingIndex, NotifyCollectionChangedEventArgs eventArgs)
        {
            Assert.AreEqual(NotifyCollectionChangedAction.Add, eventArgs.Action);
            Assert.AreEqual(newItem, eventArgs.NewItems.Cast<T>().Single());
            Assert.AreEqual(newStartingIndex, eventArgs.NewStartingIndex);
            Assert.IsNull(eventArgs.OldItems);
            Assert.AreEqual(-1, eventArgs.OldStartingIndex);
        }

        private static void AssertElementRemoved<T>(T oldItem, int oldStartingIndex, NotifyCollectionChangedEventArgs eventArgs)
        {
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, eventArgs.Action);
            Assert.AreEqual(oldItem, eventArgs.OldItems.Cast<T>().Single());
            Assert.AreEqual(oldStartingIndex, eventArgs.OldStartingIndex);
            Assert.IsNull(eventArgs.NewItems);
            Assert.AreEqual(-1, eventArgs.NewStartingIndex);
        }

        private static void AssertElementMoved<T>(T item, int oldIndex, int newIndex, NotifyCollectionChangedEventArgs eventArgs)
        {
            Assert.AreEqual(NotifyCollectionChangedAction.Move, eventArgs.Action);
            Assert.AreEqual(item, eventArgs.NewItems.Cast<T>().Single());
            Assert.AreEqual(item, eventArgs.OldItems.Cast<T>().Single());
            Assert.AreEqual(oldIndex, eventArgs.OldStartingIndex);
            Assert.AreEqual(newIndex, eventArgs.NewStartingIndex);
        }

        private void AssertNoEventsRaised(Action action)
        {
            eventArgsList.Clear();
            countChangedCount = indexerChangedCount = 0;

            action();

            Assert.IsFalse(eventArgsList.Any());
            Assert.AreEqual(0, countChangedCount);
            Assert.AreEqual(0, indexerChangedCount);
        }
    }
}
