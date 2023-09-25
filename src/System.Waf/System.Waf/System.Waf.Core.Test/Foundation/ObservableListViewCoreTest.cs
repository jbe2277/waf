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
    public class ObservableListViewCoreTest
    {
        private ObservableList<string> originalList = null!;
        private ObservableListViewCore<string> observableListView = null!;
        private List<NotifyCollectionChangedEventArgs> collectionChangingList = null!;
        private List<NotifyCollectionChangedEventArgs> collectionChangedList = null!;
        private int countChangedCount;
        private int indexerChangedCount;

        [TestInitialize]
        public void Initialize()
        {
            originalList = new();
            observableListView = new(originalList);
            collectionChangingList = new();
            collectionChangedList = new();

            void CollectionChangingHandler(object? sender, NotifyCollectionChangedEventArgs e) => collectionChangingList.Add(e);
            observableListView.CollectionChanging += CollectionChangingHandler;

            void CollectionChangedHandler(object? sender, NotifyCollectionChangedEventArgs e) => collectionChangedList.Add(e);
            observableListView.CollectionChanged += CollectionChangedHandler;

            void PropertyHandler(object? sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(observableListView.Count)) countChangedCount++;
                else if (e.PropertyName == "Item[]") indexerChangedCount++;
            }
            observableListView.PropertyChanged += PropertyHandler;
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
            AssertElementAdded("first", 0, collectionChangingList.Single(), collectionChangedList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            ClearCollectionChangeLists();
            originalList.Add("second");
            AssertElementAdded("second", 1, collectionChangingList.Single(), collectionChangedList.Single());

            ClearCollectionChangeLists();
            originalList.Add("third");
            AssertElementAdded("third", 2, collectionChangingList.Single(), collectionChangedList.Single());

            ClearCollectionChangeLists();
            originalList.Add("four");
            AssertElementAdded("four", 3, collectionChangingList.Single(), collectionChangedList.Single());

            AssertNoEventsRaised(() => observableListView.Update());  // Collection has not changed.

            ClearCollectionChangeLists();
            originalList.Move(0, 3);
            AssertElementMoved("first", 0, 3, collectionChangingList.Single(), collectionChangedList.Single());
            ClearCollectionChangeLists();
            originalList.Move(3, 0);
            AssertElementMoved("first", 3, 0, collectionChangingList.Single(), collectionChangedList.Single());

            ClearCollectionChangeLists();
            originalList.Move(3, 1);
            AssertElementMoved("four", 3, 1, collectionChangingList.Single(), collectionChangedList.Single());
            ClearCollectionChangeLists();
            originalList.Move(1, 3);
            AssertElementMoved("four", 1, 3, collectionChangingList.Single(), collectionChangedList.Single());

            ClearCollectionChangeLists();
            originalList.Move(2, 3);
            AssertElementMoved("third", 2, 3, collectionChangingList.Single(), collectionChangedList.Single());
            ClearCollectionChangeLists();
            originalList.Move(3, 2);
            AssertElementMoved("four", 2, 3, collectionChangingList.Single(), collectionChangedList.Single());

            ClearCollectionChangeLists();
            countChangedCount = indexerChangedCount = 0;
            originalList.Clear();
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangingList.Single().Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedList.Single().Action);
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            ClearCollectionChangeLists();
            originalList.Insert(0, "1");
            AssertElementAdded("1", 0, collectionChangingList.Single(), collectionChangedList.Single());

            ClearCollectionChangeLists();
            originalList.Insert(1, "3");
            AssertElementAdded("3", 1, collectionChangingList.Single(), collectionChangedList.Single());

            ClearCollectionChangeLists();
            originalList.Insert(1, "2");
            AssertElementAdded("2", 1, collectionChangingList.Single(), collectionChangedList.Single());

            ClearCollectionChangeLists();
            countChangedCount = indexerChangedCount = 0;
            originalList.Remove("1");
            AssertElementRemoved("1", 0, collectionChangingList.Single(), collectionChangedList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            ClearCollectionChangeLists();
            originalList.RemoveAt(1);
            AssertElementRemoved("3", 1, collectionChangingList.Single(), collectionChangedList.Single());

            observableListView.Dispose();
            ClearCollectionChangeLists();
            AssertNoEventsRaised(() => originalList.Clear());
            Assert.AreEqual(0, collectionChangingList.Count);
            Assert.AreEqual(0, collectionChangedList.Count);

            observableListView.Dispose();
        }

        [TestMethod]
        public void RelayEventsWithFilter()
        {
            observableListView.Filter = item => item != "second" && item != "2";

            ClearCollectionChangeLists();
            originalList.Add("first");
            AssertElementAdded("first", 0, collectionChangingList.Single(), collectionChangedList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            AssertNoEventsRaised(() => originalList.Add("second"));

            ClearCollectionChangeLists();
            originalList.Add("third");
            AssertElementAdded("third", 1, collectionChangingList.Single(), collectionChangedList.Single());

            AssertNoEventsRaised(() => observableListView.Update());  // Collection has not changed.

            AssertHelper.SequenceEqual(new[] { "first", "third" }, observableListView);

            ClearCollectionChangeLists();
            countChangedCount = indexerChangedCount = 0;
            originalList.Clear();
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangingList.Single().Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedList.Single().Action);
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            ClearCollectionChangeLists();
            originalList.Insert(0, "1");
            AssertElementAdded("1", 0, collectionChangingList.Single(), collectionChangedList.Single());

            ClearCollectionChangeLists();
            originalList.Insert(1, "3");
            AssertElementAdded("3", 1, collectionChangingList.Single(), collectionChangedList.Single());

            AssertNoEventsRaised(() => originalList.Insert(1, "2"));

            AssertHelper.SequenceEqual(new[] { "1", "3" }, observableListView);

            ClearCollectionChangeLists();
            countChangedCount = indexerChangedCount = 0;
            originalList.Remove("1");
            AssertElementRemoved("1", 0, collectionChangingList.Single(), collectionChangedList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            ClearCollectionChangeLists();
            originalList.RemoveAt(1);
            AssertElementRemoved("3", 0, collectionChangingList.Single(), collectionChangedList.Single());  // Index 0 because "2" is hidden by filter
        }

        [TestMethod]
        public void RelayEventsWithSort()
        {
            observableListView.Sort = x => x.OrderBy(y => y);

            ClearAll();
            originalList.Add("c");
            AssertElementAdded("c", 0, collectionChangingList.Single(), collectionChangedList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            ClearAll();
            originalList.Add("a");
            AssertElementAdded("a", 0, collectionChangingList.Single(), collectionChangedList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            ClearAll();
            originalList.Add("b");
            AssertElementAdded("b", 1, collectionChangingList.Single(), collectionChangedList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            AssertHelper.SequenceEqual(new[] { "a", "b", "c" }, observableListView);

            AssertNoEventsRaised(() => observableListView.Update());  // Collection has not changed.

            ClearAll();
            observableListView.Sort = x => x.OrderByDescending(y => y);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangingList.Single().Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedList.Single().Action);
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);
            AssertHelper.SequenceEqual(new[] { "c", "b", "a" }, observableListView);

            ClearAll();
            originalList.Remove("b");
            AssertElementRemoved("b", 1, collectionChangingList.Single(), collectionChangedList.Single());
            Assert.AreEqual(1, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);

            ClearAll();
            observableListView.Sort = x => x.OrderBy(y => y);
            Assert.AreEqual(NotifyCollectionChangedAction.Move, collectionChangingList.Single().Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Move, collectionChangedList.Single().Action);
            Assert.AreEqual(0, countChangedCount);
            Assert.AreEqual(1, indexerChangedCount);
            AssertHelper.SequenceEqual(new[] { "a", "c" }, observableListView);
        }

        [TestMethod]
        public void RelayEventsWithDefer()
        {
            using (var deferral = observableListView.DeferCollectionChangedNotifications())
            {
                // do nothing with the original list
            }
            Assert.AreEqual(0, collectionChangingList.Count);
            Assert.AreEqual(0, collectionChangedList.Count);
            Assert.AreEqual(0, countChangedCount);
            Assert.AreEqual(0, indexerChangedCount);

            using (var deferral1 = observableListView.DeferCollectionChangedNotifications())
            {
                using (var deferral2 = observableListView.DeferCollectionChangedNotifications())
                {
                    originalList.Add("first");
                    Assert.AreEqual(1, collectionChangingList.Count);
                    Assert.AreEqual(0, collectionChangedList.Count);
                    Assert.AreEqual(1, countChangedCount);
                    Assert.AreEqual(1, indexerChangedCount);
                    originalList.Add("second");
                    Assert.AreEqual(2, countChangedCount);
                    Assert.AreEqual(2, indexerChangedCount);
                    originalList.Move(0, 1);
                    originalList.RemoveAt(0);
                    originalList.Add("third");

                    countChangedCount = indexerChangedCount = 0;
                    Assert.AreEqual(1, collectionChangingList.Count);
                    Assert.AreEqual(0, collectionChangedList.Count);
                }
                Assert.AreEqual(1, collectionChangingList.Count);
                Assert.AreEqual(0, collectionChangedList.Count);
                deferral1.Dispose();  // call Dispose twice to see if this works too
            }
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangingList.Single().Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedList.Single().Action);
            Assert.AreEqual(0, countChangedCount);
            Assert.AreEqual(0, indexerChangedCount);
        }

        [TestMethod]
        public void CollectionEventsTest()
        {
            var list = new ObservableList<CollectionEventsTestModel>();
            var listView = new ObservableListViewCore<CollectionEventsTestModel>(list);
            ObservableListTest.CollectionEventsTestCore(list, listView);
        }

        [TestMethod]
        public void RaiseEventsWithoutListener()
        {
            var originalList2 = new ObservableCollection<string>();
            var observableListView2 = new ObservableListViewCore<string>(originalList);
            originalList2.Add("first");
            originalList2.Add("second");
            originalList2.Move(0, 1);
            originalList2.Remove("first");
            originalList2.Add("third");
            originalList2.Clear();

            observableListView2.Dispose();
            observableListView2.Dispose();
        }

        [TestMethod]
        public void ConstructorTest()
        {
            var originalList = new ObservableCollection<string>(new[] { "D", "A", "c", "b" });
            Predicate<string> filter = x => x != "c";
            Func<IEnumerable<string>, IOrderedEnumerable<string>> sort = x => x.OrderBy(y => y);
            var listView = new ObservableListViewCore<string>(originalList, StringComparer.OrdinalIgnoreCase);
            listView.Filter = filter;
            listView.Sort = sort;
            AssertHelper.SequenceEqual(new[] { "A", "b", "D" }, listView);
            listView.Dispose();

            listView = new ObservableListViewCore<string>(originalList, StringComparer.OrdinalIgnoreCase, filter, sort);
            AssertHelper.SequenceEqual(new[] { "A", "b", "D" }, listView);
            Assert.AreSame(filter, listView.Filter);
            Assert.AreSame(sort, listView.Sort);
            listView.Dispose();

            listView = new ObservableListViewCore<string>(originalList, StringComparer.OrdinalIgnoreCase, filter, sort, true);
            AssertHelper.SequenceEqual(new[] { "A", "b", "D" }, listView);
            Assert.AreSame(filter, listView.Filter);
            Assert.AreSame(sort, listView.Sort);

            bool collectionChangedCalled = false;
            listView.CollectionChanged += (sender, e) =>
            {
                collectionChangedCalled = true;
            };
            originalList.Add("e");
            Assert.IsFalse(collectionChangedCalled);
            listView.Dispose();

            AssertHelper.ExpectedException<ArgumentNullException>(() => new ObservableListViewCore<string>(null!));
        }

        private void ClearAll() { ClearCollectionChangeLists(); (countChangedCount, indexerChangedCount) = (0, 0); }

        private void ClearCollectionChangeLists() { collectionChangingList.Clear(); collectionChangedList.Clear(); }

        private static void AssertElementAdded<T>(T newItem, int newStartingIndex, params NotifyCollectionChangedEventArgs[] eventArgsList)
        {
            foreach (var x in eventArgsList) AssertElementAddedCore(newItem, newStartingIndex, x);

            static void AssertElementAddedCore(T newItem, int newStartingIndex, NotifyCollectionChangedEventArgs eventArgs)
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, eventArgs.Action);
                Assert.AreEqual(newItem, eventArgs.NewItems!.Cast<T>().Single());
                Assert.AreEqual(newStartingIndex, eventArgs.NewStartingIndex);
                Assert.IsNull(eventArgs.OldItems);
                Assert.AreEqual(-1, eventArgs.OldStartingIndex);
            }
        }

        private static void AssertElementRemoved<T>(T oldItem, int oldStartingIndex, params NotifyCollectionChangedEventArgs[] eventArgsList)
        {
            foreach (var x in eventArgsList) AssertElementRemovedCore(oldItem, oldStartingIndex, x);

            static void AssertElementRemovedCore(T oldItem, int oldStartingIndex, NotifyCollectionChangedEventArgs eventArgs)
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, eventArgs.Action);
                Assert.AreEqual(oldItem, eventArgs.OldItems!.Cast<T>().Single());
                Assert.AreEqual(oldStartingIndex, eventArgs.OldStartingIndex);
                Assert.IsNull(eventArgs.NewItems);
                Assert.AreEqual(-1, eventArgs.NewStartingIndex);
            }
        }

        private static void AssertElementMoved<T>(T item, int oldIndex, int newIndex, params NotifyCollectionChangedEventArgs[] eventArgsList)
        {
            foreach (var x in eventArgsList) AssertElementMovedCore(item, oldIndex, newIndex, x);

            static void AssertElementMovedCore(T item, int oldIndex, int newIndex, NotifyCollectionChangedEventArgs eventArgs)
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Move, eventArgs.Action);
                Assert.AreEqual(item, eventArgs.NewItems!.Cast<T>().Single());
                Assert.AreEqual(item, eventArgs.OldItems!.Cast<T>().Single());
                Assert.AreEqual(oldIndex, eventArgs.OldStartingIndex);
                Assert.AreEqual(newIndex, eventArgs.NewStartingIndex);
            }
        }

        private void AssertNoEventsRaised(Action action)
        {
            ClearAll();

            action();

            Assert.AreEqual(0, collectionChangingList.Count);
            Assert.AreEqual(0, collectionChangedList.Count);
            Assert.AreEqual(0, countChangedCount);
            Assert.AreEqual(0, indexerChangedCount);
        }
    }
}
