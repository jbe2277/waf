using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Test.BookLibrary.Library.Applications.Views;
using System.Waf.UnitTesting;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.ViewModels
{
    [TestClass]
    public class LendToViewModelTest
    {
        [TestMethod]
        public void LendToViewModelLendToTest()
        {
            Book book = new Book() { Title = "The Fellowship of the Ring" };
            
            List<Person> persons = new List<Person>()
            {
                new Person() { Firstname = "Harry" },
                new Person() { Firstname = "Ron" }
            };
            
            MockLendToView lendToView = new MockLendToView();
            LendToViewModel lendToViewModel = new LendToViewModel(lendToView) { Book = book, Persons = persons };

            Assert.AreEqual(book, lendToViewModel.Book);
            Assert.AreEqual(persons, lendToViewModel.Persons);
            
            // Show the dialog
            object owner = new object();
            Action<MockLendToView> showDialogAction = (view) =>
            {
                Assert.AreEqual("", lendToViewModel.Title);
                Assert.IsTrue(lendToView.IsVisible);
                Assert.AreEqual(owner, lendToView.Owner);

                // Check the default values
                Assert.IsTrue(lendToViewModel.IsLendTo);
                Assert.IsFalse(lendToViewModel.IsWasReturned);
                Assert.AreEqual(persons.First(), lendToViewModel.SelectedPerson);

                // Select the last person: Lend to Ron
                AssertHelper.PropertyChangedEvent(lendToViewModel, x => x.SelectedPerson, () =>
                    lendToViewModel.SelectedPerson = persons.Last());

                // Press Ok button
                lendToViewModel.OkCommand.Execute(null);
            };

            MockLendToView.ShowDialogAction = showDialogAction;
            Assert.IsTrue(lendToViewModel.ShowDialog(owner));
            Assert.IsFalse(lendToView.IsVisible);
            Assert.AreEqual(persons.Last(), lendToViewModel.SelectedPerson);

            MockLendToView.ShowDialogAction = null;
        }

        [TestMethod]
        public void LendToViewModelWasReturnedTest()
        {
            List<Person> persons = new List<Person>()
            {
                new Person() { Firstname = "Harry" },
                new Person() { Firstname = "Ron" }
            };
            
            Book book = new Book() { Title = "The Fellowship of the Ring", LendTo = persons.First() };

            MockLendToView lendToView = new MockLendToView();
            LendToViewModel lendToViewModel = new LendToViewModel(lendToView) { Book = book, Persons = persons };

            // Show the dialog
            object owner = new object();
            Action<MockLendToView> showDialogAction = (view) =>
            {
                // Check the default values
                Assert.IsFalse(lendToViewModel.IsLendTo);
                Assert.IsTrue(lendToViewModel.IsWasReturned);

                // Change check boxes
                AssertHelper.PropertyChangedEvent(lendToViewModel, x => x.IsLendTo, () => 
                    lendToViewModel.IsLendTo = true);
                Assert.IsTrue(lendToViewModel.IsLendTo);
                Assert.IsFalse(lendToViewModel.IsWasReturned);

                // Restore the original check boxes state
                AssertHelper.PropertyChangedEvent(lendToViewModel, x => x.IsWasReturned, () => 
                    lendToViewModel.IsWasReturned = true);
                Assert.IsFalse(lendToViewModel.IsLendTo);
                Assert.IsTrue(lendToViewModel.IsWasReturned);

                lendToViewModel.OkCommand.Execute(null);
            };

            MockLendToView.ShowDialogAction = showDialogAction;
            Assert.IsNotNull(lendToViewModel.SelectedPerson);
            Assert.IsTrue(lendToViewModel.ShowDialog(owner));
            Assert.IsNull(lendToViewModel.SelectedPerson);

            MockLendToView.ShowDialogAction = null;
        }
    }
}
