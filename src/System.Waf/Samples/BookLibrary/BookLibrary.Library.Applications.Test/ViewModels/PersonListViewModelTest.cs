using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Waf.UnitTesting;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.ViewModels
{
    [TestClass]
    public class PersonListViewModelTest : TestClassBase
    {
        [TestMethod]
        public void PersonListViewModelPersonsTest()
        {
            var persons = new List<Person>()
            {
                new Person() { Firstname = "Harry" },
                new Person() { Firstname = "Ron" }
            };
            var personListView = new MockPersonListView();
            var personListViewModel = new PersonListViewModel(personListView) { Persons = persons };

            Assert.AreEqual(persons, personListViewModel.Persons);
            Assert.IsNull(personListViewModel.SelectedPerson);
            Assert.IsFalse(personListViewModel.SelectedPersons.Any());

            // Select the first person
            AssertHelper.PropertyChangedEvent(personListViewModel, x => x.SelectedPerson, () => personListViewModel.SelectedPerson = persons[0]);
            Assert.AreEqual(persons[0], personListViewModel.SelectedPerson);
            
            personListViewModel.AddSelectedPerson(persons[0]);
            AssertHelper.SequenceEqual(new[] { persons[0] }, personListViewModel.SelectedPersons);

            // Select both persons
            personListViewModel.AddSelectedPerson(persons[^1]);
            AssertHelper.SequenceEqual(persons, personListViewModel.SelectedPersons);
        }

        [TestMethod]
        public void PersonListViewModelFilterTest()
        {
            var persons = new List<Person>()
            {
                new Person() { Firstname = "Harry", Lastname = "Potter" },
                new Person() { Firstname = "Ron", Lastname = "Weasley" }
            };
            var personListView = new MockPersonListView();
            var personListViewModel = new PersonListViewModel(personListView) { Persons = persons };

            Assert.IsTrue(personListViewModel.Filter(persons[0]));
            Assert.IsTrue(personListViewModel.Filter(persons[1]));

            AssertHelper.PropertyChangedEvent(personListViewModel, x => x.FilterText, () => personListViewModel.FilterText = "r");
            Assert.AreEqual("r", personListViewModel.FilterText);
            Assert.IsTrue(personListViewModel.Filter(persons[0]));
            Assert.IsTrue(personListViewModel.Filter(persons[1]));

            personListViewModel.FilterText = "arr";
            Assert.IsTrue(personListViewModel.Filter(persons[0]));
            Assert.IsFalse(personListViewModel.Filter(persons[1]));

            personListViewModel.FilterText = "eas";
            Assert.IsFalse(personListViewModel.Filter(persons[0]));
            Assert.IsTrue(personListViewModel.Filter(persons[1]));

            personListViewModel.FilterText = "xyz";
            Assert.IsFalse(personListViewModel.Filter(persons[0]));
            Assert.IsFalse(personListViewModel.Filter(persons[1]));

            personListViewModel.FilterText = "R";
            persons.Add(new Person());
            Assert.IsFalse(personListViewModel.Filter(persons[2]));
            persons[2].Firstname = "Hermione";
            Assert.IsTrue(personListViewModel.Filter(persons[2]));
        }
    }
}
