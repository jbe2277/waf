using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using System.Waf.Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class CollectionHelperTest
    {
        [TestMethod]
        public void EmptyTest()
        {
            var emptyStringList1 = CollectionHelper.Empty<string>();
            Assert.AreEqual(0, emptyStringList1.Count);

            // Check that the list is cached
            var emptyStringList2 = CollectionHelper.Empty<string>();
            Assert.AreSame(emptyStringList1, emptyStringList2);

            var emptyIntList1 = CollectionHelper.Empty<int>();
            Assert.AreEqual(0, emptyIntList1.Count);
            var emptyIntList2 = CollectionHelper.Empty<int>();
            Assert.AreSame(emptyIntList1, emptyIntList2);
        }

        [TestMethod]
        public void IndexOfTest()
        {
            IReadOnlyList<string> collection1 = new[] { "Zero", "One", "Two" };
            Assert.AreEqual(0, collection1.IndexOf("Zero"));
            Assert.AreEqual(1, collection1.IndexOf("One"));
            Assert.AreEqual(2, collection1.IndexOf("Two"));
            Assert.AreEqual(-1, collection1.IndexOf("Nine"));
            Assert.AreEqual(-1, collection1.IndexOf(null));

            var collection2 = new Queue<string>(new[] { "Zero", "One", "Two" });
            Assert.AreEqual(0, collection2.IndexOf("Zero"));
            Assert.AreEqual(1, collection2.IndexOf("One"));
            Assert.AreEqual(2, collection2.IndexOf("Two"));
            Assert.AreEqual(-1, collection2.IndexOf("Nine"));
            Assert.AreEqual(-1, collection2.IndexOf(null));
        }

        [TestMethod]
        public void GetNextElementOrDefaultTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => CollectionHelper.GetNextElementOrDefault(null, 5));

            int[] collection1 = new[] { 1, 2, 3, 4, 5 };
            Assert.AreEqual(4, CollectionHelper.GetNextElementOrDefault(collection1, 3));
            Assert.AreEqual(2, CollectionHelper.GetNextElementOrDefault(collection1, 1));
            Assert.AreEqual(0, CollectionHelper.GetNextElementOrDefault(collection1, 5));

            int[] collection2 = new[] { 1 };
            Assert.AreEqual(0, CollectionHelper.GetNextElementOrDefault(collection2, 1));

            int[] collection3 = new int[] { };
            AssertHelper.ExpectedException<ArgumentException>(() => CollectionHelper.GetNextElementOrDefault(collection3, 9));
        }

        [TestMethod]
        public void MergeTestWithDefaultArguments()
        {
            var targetList = new List<string> { "2", "3" };
            targetList.Merge(new[] { "1", "2" });
            Assert.IsTrue(new[] { "1", "2" }.SequenceEqual(targetList));

            targetList.Merge(new[] { "1", "2" });
            Assert.IsTrue(new[] { "1", "2" }.SequenceEqual(targetList));

            targetList.Merge(new[] { "1", "3" });
            Assert.IsTrue(new[] { "1", "3" }.SequenceEqual(targetList));

            targetList.Merge(new[] { "1", "2", "3" });
            Assert.IsTrue(new[] { "1", "2", "3" }.SequenceEqual(targetList));

            targetList.Merge(new[] { "1", "2", "c", "d" });
            Assert.IsTrue(new[] { "1", "2", "c", "d" }.SequenceEqual(targetList));

            targetList.Merge(new[] { "a", "b", "c" });
            Assert.IsTrue(new[] { "a", "b", "c" }.SequenceEqual(targetList));

            targetList.Merge(new[] { "a", "b" });
            Assert.IsTrue(new[] { "a", "b" }.SequenceEqual(targetList));

            targetList.Merge(new[] { "1" });
            Assert.IsTrue(new[] { "1" }.SequenceEqual(targetList));

            targetList.Merge(new string[0]);
            Assert.IsTrue(new string[0].SequenceEqual(targetList));

            targetList.Merge(Array.AsReadOnly(new[] { "1" }));
            Assert.IsTrue(new[] { "1" }.SequenceEqual(targetList));
        }

        [TestMethod]
        public void MergeTest()
        {
            MergeTestCore(new[] { "1", "2" }, new[] { "1", "2" }.ToList(), ExpectedCollectionChange.None);

            MergeTestCore(new[] { "1", "2", "3" }, new[] { "1", "2" }.ToList(), ExpectedCollectionChange.Insert);
            MergeTestCore(new[] { "1", "2", "3" }, new[] { "1", "3" }.ToList(), ExpectedCollectionChange.Insert);
            MergeTestCore(new[] { "1", "2", "3" }, new[] { "2", "3" }.ToList(), ExpectedCollectionChange.Insert);

            MergeTestCore(new[] { "1", "2" }, new[] { "1", "2", "3" }.ToList(), ExpectedCollectionChange.Remove);
            MergeTestCore(new[] { "1", "3" }, new[] { "1", "2", "3" }.ToList(), ExpectedCollectionChange.Remove);
            MergeTestCore(new[] { "2", "3" }, new[] { "1", "2", "3" }.ToList(), ExpectedCollectionChange.Remove);

            MergeTestCore(new[] { "1", "2" }, new[] { "2", "3" }.ToList(), ExpectedCollectionChange.Reset);
            MergeTestCore(new[] { "2", "4", "5" }, new[] { "2", "3" }.ToList(), ExpectedCollectionChange.Reset);
            MergeTestCore(new[] { "4" }, new[] { "2", "3" }.ToList(), ExpectedCollectionChange.Reset);

            MergeTestCore(new[] { "1", "2" }, new[] { "2", "1" }.ToList(), ExpectedCollectionChange.None, null, 1);
            MergeTestCore(new[] { "1", "2", "3" }, new[] { "3", "1", "2" }.ToList(), ExpectedCollectionChange.None, null, 1);
            MergeTestCore(new[] { "1", "2", "3", "4" }, new[] { "4", "3", "2", "1" }.ToList(), ExpectedCollectionChange.None, null, 3);
            MergeTestCore(new[] { "1", "2" }, new[] { "1" }.ToList(), ExpectedCollectionChange.Insert, null, 0);
            MergeTestCore(new[] { "1", "2" }, new[] { "1", "3" }.ToList(), ExpectedCollectionChange.Reset, null, 0);
        }

        [TestMethod]
        public void MergeTestWithComparer()
        {
            MergeTestCore(new[] { "a", "b" }, new[] { "A", "B" }.ToList(), ExpectedCollectionChange.None, StringComparer.OrdinalIgnoreCase);

            MergeTestCore(new[] { "a", "b", "c" }, new[] { "A", "B" }.ToList(), ExpectedCollectionChange.Insert, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "a", "b", "c" }, new[] { "A", "C" }.ToList(), ExpectedCollectionChange.Insert, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "a", "b", "c" }, new[] { "B", "C" }.ToList(), ExpectedCollectionChange.Insert, StringComparer.OrdinalIgnoreCase);

            MergeTestCore(new[] { "a", "b" }, new[] { "A", "B", "C" }.ToList(), ExpectedCollectionChange.Remove, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "a", "c" }, new[] { "A", "B", "C" }.ToList(), ExpectedCollectionChange.Remove, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "b", "c" }, new[] { "A", "B", "C" }.ToList(), ExpectedCollectionChange.Remove, StringComparer.OrdinalIgnoreCase);

            MergeTestCore(new[] { "a", "b" }, new[] { "B", "C" }.ToList(), ExpectedCollectionChange.Reset, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "b", "d", "e" }, new[] { "B", "C" }.ToList(), ExpectedCollectionChange.Reset, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "d" }, new[] { "B", "C" }.ToList(), ExpectedCollectionChange.Reset, StringComparer.OrdinalIgnoreCase);

            MergeTestCore(new[] { "a", "b" }, new[] { "B", "A" }.ToList(), ExpectedCollectionChange.None, StringComparer.OrdinalIgnoreCase, 1);
            MergeTestCore(new[] { "a", "b", "c" }, new[] { "C", "A", "B" }.ToList(), ExpectedCollectionChange.None, StringComparer.OrdinalIgnoreCase, 1);
            MergeTestCore(new[] { "a", "b", "c", "d" }, new[] { "D", "C", "B", "A" }.ToList(), ExpectedCollectionChange.None, StringComparer.OrdinalIgnoreCase, 3);
            MergeTestCore(new[] { "a", "b" }, new[] { "A" }.ToList(), ExpectedCollectionChange.Insert, StringComparer.OrdinalIgnoreCase, 0);
            MergeTestCore(new[] { "a", "b" }, new[] { "A", "C" }.ToList(), ExpectedCollectionChange.Reset, StringComparer.OrdinalIgnoreCase, 0);
        }

        private void MergeTestCore<T>(IList<T> source, IList<T> target, ExpectedCollectionChange expectedCollectionChange, IEqualityComparer<T> comparer = null, int expectedMoveCounts = -1)
        {
            var insertCounter = 0;
            var removeCounter = 0;
            var resetCounter = 0;
            var moveCounter = 0;
            Action<int, T> insertAction = (idx, item) => 
            {
                target.Insert(idx, item);
                insertCounter++;
            };
            Action<int> removeAction = idx =>
            {
                target.RemoveAt(idx);
                removeCounter++;
            };
            Action resetAction = () =>
            {
                target.Clear();
                foreach (var item in source) target.Add(item);
                resetCounter++;
            };
            Action<int, int> moveAction = null;
            if (expectedMoveCounts >= 0) moveAction = (int oldIndex, int newIndex) =>
            {
                T item = target[oldIndex];
                target.RemoveAt(oldIndex);
                target.Insert(newIndex, item);
                moveCounter++;
            };

            target.Merge(new ReadOnlyCollection<T>(source), comparer, insertAction, removeAction, resetAction, moveAction);
            Assert.IsTrue(target.SequenceEqual(source, comparer ?? EqualityComparer<T>.Default));
            Assert.AreEqual(expectedCollectionChange == ExpectedCollectionChange.Insert ? 1 : 0, insertCounter);
            Assert.AreEqual(expectedCollectionChange == ExpectedCollectionChange.Remove ? 1 : 0, removeCounter);
            Assert.AreEqual(expectedCollectionChange == ExpectedCollectionChange.Reset ? 1 : 0, resetCounter);
            if (expectedMoveCounts >= 0) Assert.AreEqual(expectedMoveCounts, moveCounter);
        }

        private enum ExpectedCollectionChange
        {
            None,
            Insert,
            Remove,
            Reset
        }
    }
}
