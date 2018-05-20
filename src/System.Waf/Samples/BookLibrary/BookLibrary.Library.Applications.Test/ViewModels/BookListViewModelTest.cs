using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Test.BookLibrary.Library.Applications.Views;
using System.Waf.UnitTesting;
using Waf.BookLibrary.Library.Domain;
using System.Waf.Applications;
using Waf.BookLibrary.Library.Applications.DataModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Test.BookLibrary.Library.Applications.ViewModels
{
    [TestClass]
    public class BookListViewModelTest
    {
        private static readonly ICommand dummyCommand = new DelegateCommand(() => { });

        
        [TestMethod]
        public void BookListViewModelBooksTest()
        {
            List<Book> books = new List<Book>()
            {
                new Book() { Title = "The Fellowship of the Ring" },
                new Book() { Title = "The Two Towers" }
            };
            
            MockBookListView bookListView = new MockBookListView();
            var bookDataModels = new SynchronizingCollection<BookDataModel, Book>(books, b => new BookDataModel(b, dummyCommand));

            BookListViewModel bookListViewModel = new BookListViewModel(bookListView) { Books = bookDataModels };

            Assert.AreEqual(bookDataModels, bookListViewModel.Books);
            Assert.IsNull(bookListViewModel.SelectedBook);
            Assert.IsFalse(bookListViewModel.SelectedBooks.Any());

            // Select the first book
            AssertHelper.PropertyChangedEvent(bookListViewModel, x => x.SelectedBook,
                () => bookListViewModel.SelectedBook = bookDataModels.First());
            Assert.AreEqual(books.First(), bookListViewModel.SelectedBook.Book);

            bookListViewModel.AddSelectedBook(bookDataModels.First());
            Assert.IsTrue(bookListViewModel.SelectedBooks.SequenceEqual(new[] { bookDataModels.First() }));

            // Select both books
            bookListViewModel.AddSelectedBook(bookDataModels.Last());
            Assert.IsTrue(bookListViewModel.SelectedBooks.SequenceEqual(bookDataModels));
        }

        [TestMethod]
        public void BookListViewModelFilterTest()
        {
            IList<Book> books = new ObservableCollection<Book>()
            {
                new Book() { Title = "The Fellowship of the Ring", Author = "J.R.R. Tolkien" },
                new Book() { Title = "The Two Towers", Author = "J.R.R. Tolkien" }
            };

            MockBookListView bookListView = new MockBookListView();
            var bookDataModels = new SynchronizingCollection<BookDataModel, Book>(books, b => new BookDataModel(b, dummyCommand));
            BookListViewModel bookListViewModel = new BookListViewModel(bookListView) { Books = bookDataModels };

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

            books.Add(new Book());
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[2]));
            books[2].Title = "Serenity, Vol 1: Those Left Behind";
            Assert.IsTrue(bookListViewModel.Filter(bookDataModels[2]));
        }
    }
}
