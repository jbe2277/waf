using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using System.Windows.Input;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.ViewModels
{
    [TestClass]
    public class BookListViewModelTest
    {
        private static readonly ICommand dummyCommand = new DelegateCommand(() => { });

        
        [TestMethod]
        public void BookListViewModelBooksTest()
        {
            var books = new List<Book>()
            {
                new Book() { Title = "The Fellowship of the Ring" },
                new Book() { Title = "The Two Towers" }
            };
            var bookListView = new MockBookListView();
            var bookDataModels = new SynchronizingCollection<BookDataModel, Book>(books, b => new BookDataModel(b, dummyCommand));
            var bookListViewModel = new BookListViewModel(bookListView) { Books = bookDataModels };

            Assert.AreEqual(bookDataModels, bookListViewModel.Books);
            Assert.IsNull(bookListViewModel.SelectedBook);
            Assert.IsFalse(bookListViewModel.SelectedBooks.Any());

            // Select the first book
            AssertHelper.PropertyChangedEvent(bookListViewModel, x => x.SelectedBook,
                () => bookListViewModel.SelectedBook = bookDataModels.First());
            Assert.AreEqual(books.First(), bookListViewModel.SelectedBook.Book);

            bookListViewModel.AddSelectedBook(bookDataModels.First());
            CollectionAssert.AreEqual(new[] { bookDataModels.First() }, bookListViewModel.SelectedBooks.ToArray());

            // Select both books
            bookListViewModel.AddSelectedBook(bookDataModels.Last());
            CollectionAssert.AreEqual(bookDataModels, bookListViewModel.SelectedBooks.ToArray());
        }

        [TestMethod]
        public void BookListViewModelFilterTest()
        {
            var books = new ObservableCollection<Book>()
            {
                new Book() { Title = "The Fellowship of the Ring", Author = "J.R.R. Tolkien" },
                new Book() { Title = "The Two Towers", Author = "J.R.R. Tolkien" }
            };
            var bookListView = new MockBookListView();
            var bookDataModels = new SynchronizingCollection<BookDataModel, Book>(books, b => new BookDataModel(b, dummyCommand));
            var bookListViewModel = new BookListViewModel(bookListView) { Books = bookDataModels };

            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[0]));
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[1]));

            AssertHelper.PropertyChangedEvent(bookListViewModel, x => x.FilterText, () => bookListViewModel.FilterText = "J.");
            Assert.AreEqual("J.", bookListViewModel.FilterText);
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[0]));
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[1]));

            bookListViewModel.FilterText = "J.R.R. Tolkien";
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[0]));
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[1]));

            bookListViewModel.FilterText = "Fell";
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[0]));
            Assert.IsFalse(bookListViewModel.Filter(bookDataModels[1]));

            bookListViewModel.FilterText = "Tow";
            Assert.IsFalse(bookListViewModel.Filter(bookDataModels[0]));
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[1]));

            bookListViewModel.FilterText = "xyz";
            Assert.IsFalse(bookListViewModel.Filter(bookDataModels[0]));
            Assert.IsFalse(bookListViewModel.Filter(bookDataModels[1]));

            bookListViewModel.FilterText = "vol";
            books.Add(new Book());
            Assert.IsFalse(bookListViewModel.Filter(bookDataModels[2]));
            books[2].Title = "Serenity, Vol 1: Those Left Behind";
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[2]));
        }
    }
}
