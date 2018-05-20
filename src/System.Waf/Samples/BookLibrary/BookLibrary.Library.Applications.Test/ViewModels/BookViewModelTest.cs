using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;
using System.Waf.UnitTesting;
using System.Waf.Applications;

namespace Test.BookLibrary.Library.Applications.ViewModels
{
    [TestClass]
    public class BookViewModelTest
    {
        [TestMethod]
        public void BookViewModelBookTest()
        {
            MockBookView bookView = new MockBookView();
            BookViewModel bookViewModel = new BookViewModel(bookView);

            Assert.IsFalse(bookViewModel.IsEnabled);

            Book book = new Book();
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
            MockBookView bookView = new MockBookView();
            BookViewModel bookViewModel = new BookViewModel(bookView);

            Assert.IsTrue(bookViewModel.IsValid);

            AssertHelper.PropertyChangedEvent(bookViewModel, x => x.IsValid, () => bookViewModel.IsValid = false);
            Assert.IsFalse(bookViewModel.IsValid);
        }
    }
}
