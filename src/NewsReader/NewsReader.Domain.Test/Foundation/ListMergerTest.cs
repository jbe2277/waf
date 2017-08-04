using Jbe.NewsReader.Domain.Foundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Test.NewsReader.Domain.UnitTesting;

namespace Test.NewsReader.Domain.Foundation
{
    [TestClass]
    public class ListMergerTest : DomainTest
    {
        [TestMethod]
        public void MergeTestWithDefaultArguments()
        {
            var targetList = new List<string> { "2", "3" };
            ListMerger.Merge(new[] { "1", "2" }, targetList);
            Assert.IsTrue(new[] { "1", "2" }.SequenceEqual(targetList));

            ListMerger.Merge(new[] { "1", "2" }, targetList);
            Assert.IsTrue(new[] { "1", "2" }.SequenceEqual(targetList));

            ListMerger.Merge(new[] { "1", "3" }, targetList);
            Assert.IsTrue(new[] { "1", "3" }.SequenceEqual(targetList));

            ListMerger.Merge(new[] { "1", "2", "3" }, targetList);
            Assert.IsTrue(new[] { "1", "2", "3" }.SequenceEqual(targetList));

            ListMerger.Merge(new[] { "1", "2", "c", "d" }, targetList);
            Assert.IsTrue(new[] { "1", "2", "c", "d" }.SequenceEqual(targetList));
            
            ListMerger.Merge(new[] { "a", "b", "c" }, targetList);
            Assert.IsTrue(new[] { "a", "b", "c" }.SequenceEqual(targetList));

            ListMerger.Merge(new[] { "a", "b" }, targetList);
            Assert.IsTrue(new[] { "a", "b" }.SequenceEqual(targetList));

            ListMerger.Merge(new[] { "1" }, targetList);
            Assert.IsTrue(new[] { "1" }.SequenceEqual(targetList));

            ListMerger.Merge(new string[0], targetList);
            Assert.IsTrue(new string[0].SequenceEqual(targetList));

            ListMerger.Merge(Array.AsReadOnly(new[] { "1" }), targetList);
            Assert.IsTrue(new[] { "1" }.SequenceEqual(targetList));
        }

        [TestMethod]
        public void MergeTest()
        {
            MergeTestCore(new[] { "1", "2" }, new[] { "1", "2" }, ExpectedCollectionChange.None);

            MergeTestCore(new[] { "1", "2", "3" }, new[] { "1", "2" }, ExpectedCollectionChange.Insert);
            MergeTestCore(new[] { "1", "2", "3" }, new[] { "1", "3" }, ExpectedCollectionChange.Insert);
            MergeTestCore(new[] { "1", "2", "3" }, new[] { "2", "3" }, ExpectedCollectionChange.Insert);

            MergeTestCore(new[] { "1", "2" }, new[] { "1", "2", "3" }, ExpectedCollectionChange.Remove);
            MergeTestCore(new[] { "1", "3" }, new[] { "1", "2", "3" }, ExpectedCollectionChange.Remove);
            MergeTestCore(new[] { "2", "3" }, new[] { "1", "2", "3" }, ExpectedCollectionChange.Remove);

            MergeTestCore(new[] { "1", "2" }, new[] { "2", "3" }, ExpectedCollectionChange.Reset);
            MergeTestCore(new[] { "2", "4", "5" }, new[] { "2", "3" }, ExpectedCollectionChange.Reset);
            MergeTestCore(new[] { "4" }, new[] { "2", "3" }, ExpectedCollectionChange.Reset);
        }

        [TestMethod]
        public void MergeTestWithComparer()
        {
            MergeTestCore(new[] { "a", "b" }, new[] { "A", "B" }, ExpectedCollectionChange.None, StringComparer.OrdinalIgnoreCase);

            MergeTestCore(new[] { "a", "b", "c" }, new[] { "A", "B" }, ExpectedCollectionChange.Insert, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "a", "b", "c" }, new[] { "A", "C" }, ExpectedCollectionChange.Insert, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "a", "b", "c" }, new[] { "B", "C" }, ExpectedCollectionChange.Insert, StringComparer.OrdinalIgnoreCase);

            MergeTestCore(new[] { "a", "b" }, new[] { "A", "B", "C" }, ExpectedCollectionChange.Remove, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "a", "c" }, new[] { "A", "B", "C" }, ExpectedCollectionChange.Remove, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "b", "c" }, new[] { "A", "B", "C" }, ExpectedCollectionChange.Remove, StringComparer.OrdinalIgnoreCase);

            MergeTestCore(new[] { "a", "b" }, new[] { "B", "C" }, ExpectedCollectionChange.Reset, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "b", "d", "e" }, new[] { "B", "C" }, ExpectedCollectionChange.Reset, StringComparer.OrdinalIgnoreCase);
            MergeTestCore(new[] { "d" }, new[] { "B", "C" }, ExpectedCollectionChange.Reset, StringComparer.OrdinalIgnoreCase);
        }

        private void MergeTestCore<T>(IList<T> source, IList<T> target, ExpectedCollectionChange expectedCollectionChange, IEqualityComparer<T> comparer = null)
        {
            var insertCounter = 0;
            var removeCounter = 0;
            var resetCounter = 0;
            Action<int, T> insertAction = (idx, item) => insertCounter++;
            Action<int> removeAction = idx => removeCounter++;
            Action resetAction = () => resetCounter++;

            ListMerger.Merge(new ReadOnlyCollection<T>(source), target, comparer, insertAction, removeAction, resetAction);
            Assert.AreEqual(expectedCollectionChange == ExpectedCollectionChange.Insert ? 1 : 0, insertCounter);
            Assert.AreEqual(expectedCollectionChange == ExpectedCollectionChange.Remove ? 1 : 0, removeCounter);
            Assert.AreEqual(expectedCollectionChange == ExpectedCollectionChange.Reset ? 1 : 0, resetCounter);
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
