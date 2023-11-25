using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.ViewModels;

[TestClass]
public class LendToViewModelTest : TestClassBase
{
    protected override void OnCleanup()
    {
        MockLendToView.ShowDialogAction = null;
        base.OnCleanup();
    }

    [TestMethod]
    public void LendToViewModelLendToTest()
    {
        var book = new Book() { Title = "The Fellowship of the Ring" };
        var persons = new List<Person>()
        {
            new() { Firstname = "Harry" },
            new() { Firstname = "Ron" }
        };
        var lendToView = new MockLendToView();
        var lendToViewModel = new LendToViewModel(lendToView) { Book = book, Persons = persons, SelectedPerson = persons[0] };

        Assert.AreEqual(book, lendToViewModel.Book);
        Assert.AreEqual(persons, lendToViewModel.Persons);

        // Show the dialog
        var owner = new object();
        Action<MockLendToView> showDialogAction = _ =>
        {
            Assert.IsTrue(lendToView.IsVisible);
            Assert.AreEqual(owner, lendToView.Owner);

            // Check the default values
            Assert.IsTrue(lendToViewModel.IsLendTo);
            Assert.IsFalse(lendToViewModel.IsWasReturned);
            Assert.AreEqual(persons[0], lendToViewModel.SelectedPerson);

            // Select the last person: Lend to Ron
            AssertHelper.PropertyChangedEvent(lendToViewModel, x => x.SelectedPerson, () => lendToViewModel.SelectedPerson = persons[^1]);

            // Press Ok button
            lendToViewModel.OkCommand.Execute(null);
        };

        MockLendToView.ShowDialogAction = showDialogAction;
        Assert.IsTrue(lendToViewModel.ShowDialog(owner));
        Assert.IsFalse(lendToView.IsVisible);
        Assert.AreEqual(persons[^1], lendToViewModel.SelectedPerson);
    }

    [TestMethod]
    public void LendToViewModelWasReturnedTest()
    {
        var persons = new List<Person>()
        {
            new() { Firstname = "Harry" },
            new() { Firstname = "Ron" }
        };
        var book = new Book() { Title = "The Fellowship of the Ring", LendTo = persons[0] };
        var lendToView = new MockLendToView();
        var lendToViewModel = new LendToViewModel(lendToView) { Book = book, Persons = persons, SelectedPerson = persons[0] };

        // Show the dialog
        var owner = new object();
        Action<MockLendToView> showDialogAction = _ =>
        {
            // Check the default values
            Assert.IsFalse(lendToViewModel.IsLendTo);
            Assert.IsTrue(lendToViewModel.IsWasReturned);

            // Change check boxes
            AssertHelper.PropertyChangedEvent(lendToViewModel, x => x.IsLendTo, () => lendToViewModel.IsLendTo = true);
            Assert.IsTrue(lendToViewModel.IsLendTo);
            Assert.IsFalse(lendToViewModel.IsWasReturned);

            // Restore the original check boxes state
            AssertHelper.PropertyChangedEvent(lendToViewModel, x => x.IsWasReturned, () => lendToViewModel.IsWasReturned = true);
            Assert.IsFalse(lendToViewModel.IsLendTo);
            Assert.IsTrue(lendToViewModel.IsWasReturned);

            lendToViewModel.OkCommand.Execute(null);
        };

        MockLendToView.ShowDialogAction = showDialogAction;
        Assert.IsNotNull(lendToViewModel.SelectedPerson);
        Assert.IsTrue(lendToViewModel.ShowDialog(owner));
        Assert.IsNull(lendToViewModel.SelectedPerson);
    }
}
