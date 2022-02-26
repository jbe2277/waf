using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.ViewModels;

[TestClass]
public class BookViewModelTest
{
    [TestMethod]
    public void BookViewModelBookTest()
    {
        var bookView = new MockBookView();
        var bookViewModel = new BookViewModel(bookView);

        Assert.IsFalse(bookViewModel.IsEnabled);

        var book = new Book();
        AssertHelper.PropertyChangedEvent(bookViewModel, x => x.Book, () => bookViewModel.Book = book);
        Assert.AreEqual(book, bookViewModel.Book);
        Assert.IsTrue(bookViewModel.IsEnabled);

        AssertHelper.PropertyChangedEvent(bookViewModel, x => x.IsEnabled, () => bookViewModel.Book = null);
        Assert.IsNull(bookViewModel.Book);
        Assert.IsFalse(bookViewModel.IsEnabled);
    }

    [TestMethod]
    public void BookViewModelIsValidTest()
    {
        var bookView = new MockBookView();
        var bookViewModel = new BookViewModel(bookView);

        Assert.IsTrue(bookViewModel.IsValid);

        AssertHelper.PropertyChangedEvent(bookViewModel, x => x.IsValid, () => bookViewModel.IsValid = false);
        Assert.IsFalse(bookViewModel.IsValid);
    }
}
