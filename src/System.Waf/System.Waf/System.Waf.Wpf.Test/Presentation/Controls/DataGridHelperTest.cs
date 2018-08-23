using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Waf.Presentation.Controls;
using System.Waf.UnitTesting;
using System.Windows.Controls;

namespace Test.Waf.Presentation.Controls
{
    [TestClass]
    public class DataGridHelperTest
    {
        [TestMethod]
        public void HandleDataGridSortingTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => DataGridHelper.HandleDataGridSorting<object>(null));

            var list = CreateUnorderedList();
            var column = new DataGridTextColumn { SortMemberPath = "Person.Age" };

            var eventArgs = new DataGridSortingEventArgs(column);
            var sort = DataGridHelper.HandleDataGridSorting<PersonDataModel>(eventArgs);
            Assert.IsTrue(eventArgs.Handled);
            Assert.AreEqual(ListSortDirection.Ascending, column.SortDirection);
            AssertHelper.SequenceEqual(new[] { 1, 2, 3, 4 }, sort(list).Select(x => x.Person.Age));

            eventArgs = new DataGridSortingEventArgs(column);
            sort = DataGridHelper.HandleDataGridSorting<PersonDataModel>(eventArgs);
            Assert.IsTrue(eventArgs.Handled);
            Assert.AreEqual(ListSortDirection.Descending, column.SortDirection);
            AssertHelper.SequenceEqual(new[] { 4, 3, 2, 1 }, sort(list).Select(x => x.Person.Age));

            eventArgs = new DataGridSortingEventArgs(column);
            sort = DataGridHelper.HandleDataGridSorting<PersonDataModel>(eventArgs);
            Assert.IsTrue(eventArgs.Handled);
            Assert.IsNull(column.SortDirection);
            Assert.IsNull(sort);
        }

        [TestMethod]
        public void GetSortingTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => DataGridHelper.GetSorting<object>(null));

            var list = CreateUnorderedList();
            var column = new DataGridTextColumn { SortMemberPath = "Person.Age" };

            Assert.IsNull(DataGridHelper.GetSorting<PersonDataModel>(column));

            column.SortDirection = ListSortDirection.Ascending;
            AssertHelper.SequenceEqual(new[] { 1, 2, 3, 4 }, DataGridHelper.GetSorting<PersonDataModel>(column)(list).Select(x => x.Person.Age));

            column.SortDirection = ListSortDirection.Descending;
            AssertHelper.SequenceEqual(new[] { 4, 3, 2, 1 }, DataGridHelper.GetSorting<PersonDataModel>(column)(list).Select(x => x.Person.Age));

            // With primarySort
            column.SortDirection = null;
            AssertHelper.SequenceEqual(new[] { 4, 2, 1, 3 }, DataGridHelper.GetSorting<PersonDataModel>(column, x => x.OrderBy(y => y.Person.Name))(list).Select(x => x.Person.Age));

            column.SortDirection = ListSortDirection.Ascending;
            AssertHelper.SequenceEqual(new[] { 4, 1, 2, 3 }, DataGridHelper.GetSorting<PersonDataModel>(column, x => x.OrderBy(y => y.Person.Name))(list).Select(x => x.Person.Age));

            column.SortDirection = ListSortDirection.Descending;
            AssertHelper.SequenceEqual(new[] { 4, 2, 1, 3 }, DataGridHelper.GetSorting<PersonDataModel>(column, x => x.OrderBy(y => y.Person.Name))(list).Select(x => x.Person.Age));
        }

        [TestMethod]
        public void GetDefaultTest()
        {
            AssertDefaultEqual<int>();
            AssertDefaultEqual<int?>();
            AssertDefaultEqual<string>();
            AssertDefaultEqual<KeyValuePair<int, int>>();
            AssertDefaultEqual<object>();
        }

        private static void AssertDefaultEqual<T>()
        {
            Assert.AreEqual(default(T), DataGridHelper.GetDefault(typeof(T)));
        }

        [TestMethod]
        public void GetSelectorTest()
        {
            var obj = new object();
            Assert.AreSame(obj, DataGridHelper.GetSelector<object>(null)(obj));
            Assert.AreSame(obj, DataGridHelper.GetSelector<object>("")(obj));
            Assert.IsNull(DataGridHelper.GetSelector<object>("")(null));

            var personDataModels = CreatePersonDataModels();

            AssertSelectorEqual(personDataModels, x => x?.Person?.Name, "Person.Name");
            AssertSelectorEqual(personDataModels, x => x?.Person?.Age, "Person.Age");
            AssertSelectorEqual(personDataModels, x => x?.Person, "Person");
            AssertSelectorEqual(personDataModels, x => x?.Person?.Pair, "Person.Pair");
            AssertSelectorEqual(personDataModels, x => x?.Person?.Pair.Key, "Person.Pair.Key");
            AssertSelectorEqual(personDataModels, x => x?.Person?.Pair.Value, "Person.Pair.Value");
        }

        [TestMethod, TestCategory("Performance")]
        public void GetSelectorPerformanceTest1A()  // Reference
        {
            var personDataModels = CreatePersonDataModels();
            for (int i = 0; i < 1000000; i++) personDataModels.OrderBy(x => x?.Person?.Name);
        }

        [TestMethod, TestCategory("Performance")]
        public void GetSelectorPerformanceTest1B()
        {
            var personDataModels = CreatePersonDataModels();
            for (int i = 0; i < 1000000; i++)
                personDataModels.OrderBy(DataGridHelper.GetSelector<PersonDataModel>("Person.Name"));
        }

        [TestMethod, TestCategory("Performance")]
        public void GetSelectorPerformanceTest2A()  // Reference
        {
            var personDataModels = CreatePersonDataModels();
            for (int i = 0; i < 1000000; i++) personDataModels.OrderBy(x => x?.Person?.Age);
        }

        [TestMethod, TestCategory("Performance")]
        public void GetSelectorPerformanceTest2B()
        {
            var personDataModels = CreatePersonDataModels();
            for (int i = 0; i < 1000000; i++)
                personDataModels.OrderBy(DataGridHelper.GetSelector<PersonDataModel>("Person.Age"));
        }

        private static void AssertSelectorEqual<T>(IEnumerable<T> list, Func<T, object> expected, string actual)
        {
            foreach (var item in list)
            {
                Assert.AreEqual(expected(item), DataGridHelper.GetSelector<T>(actual)(item));
            }
        }

        private PersonDataModel[] CreatePersonDataModels()
        {
            return new[]
            {
                new PersonDataModel(),
                new PersonDataModel() { Person = new Person() { Name = "Bill", Age = 100, Pair = new KeyValuePair<int, string>(100, "Bill") } },
                null,
                new PersonDataModel() { Person = new Person() { Name = "Steve", Age = 50, Pair = new KeyValuePair<int, string>(50, "Steve") } },
                new PersonDataModel() { Person = new Person() }
            };
        }

        private PersonDataModel[] CreateUnorderedList()
        {
            return new[]
            {
                new PersonDataModel() { Person = new Person() { Age = 2, Name = "B" } },
                new PersonDataModel() { Person = new Person() { Age = 1, Name = "B" } },
                new PersonDataModel() { Person = new Person() { Age = 4, Name = "A" } },
                new PersonDataModel() { Person = new Person() { Age = 3, Name = "C" } },
            };
        }

        private class PersonDataModel
        {
            public Person Person { get; set; }
        }

        private class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public KeyValuePair<int, string> Pair { get; set; }
        }
    }
}
