using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using System.Windows.Input;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Controllers
{
    [TestClass]
    public class BookControllerTest : TestClassBase
    {
        [TestMethod]
        public void SelectionTest()
        {
            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(new Book() { Title = "The Fellowship of the Ring" });
            entityService.Books.Add(new Book() { Title = "The Two Towers" });
            
            BookController bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();

            // Check that Initialize shows the BookListView and BookView
            ShellService shellService = Container.GetExportedValue<ShellService>();
            Assert.IsInstanceOfType(shellService.BookListView, typeof(IBookListView));
            Assert.IsInstanceOfType(shellService.BookView, typeof(IBookView));

            // Check that the first Book is selected
            IBookListView bookListView = Container.GetExportedValue<IBookListView>();
            BookListViewModel bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            Assert.AreEqual(entityService.Books.First(), bookListViewModel.SelectedBook.Book);

            // Change the selection
            BookViewModel bookViewModel = Container.GetExportedValue<BookViewModel>();
            bookListViewModel.SelectedBook = bookListViewModel.Books.Last();
            Assert.AreEqual(entityService.Books.Last(), bookViewModel.Book);
        }

        [TestMethod]
        public void AddAndRemoveTest()
        {
            var fellowship = new Book() { Title = "The Fellowship of the Ring" };
            var twoTowers = new Book() { Title = "The Two Towers" };

            var entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            entityService.Books.Add(twoTowers);

            var bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();

            var bookListView = Container.GetExportedValue<MockBookListView>();
            var bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            bookListViewModel.BookCollectionView = bookListViewModel.Books;
            var bookView = Container.GetExportedValue<MockBookView>();
            var bookViewModel = ViewHelper.GetViewModel<BookViewModel>(bookView);

            // Add a new Book
            Assert.AreEqual(2, entityService.Books.Count);
            Assert.IsTrue(bookListViewModel.AddNewCommand.CanExecute(null));
            bookListViewModel.AddNewCommand.Execute(null);
            Assert.AreEqual(3, entityService.Books.Count);

            // Check that the new Book is selected and the first control gets the focus
            Assert.AreEqual(entityService.Books.Last(), bookViewModel.Book);
            Assert.IsTrue(bookListView.FirstCellHasFocus);

            // Simulate an invalid UI input state => the user can't add more books
            AssertHelper.CanExecuteChangedEvent(bookListViewModel.AddNewCommand, () =>
                bookViewModel.IsValid = false);
            Assert.IsFalse(bookListViewModel.AddNewCommand.CanExecute(null));

            // Remove the last two Books at once and check that the last remaining book is selected.
            bookViewModel.IsValid = true;
            bookListView.FirstCellHasFocus = false;
            bookListViewModel.AddSelectedBook(bookListViewModel.Books.Single(b => b.Book == twoTowers));
            bookListViewModel.AddSelectedBook(bookListViewModel.Books.Last());
            Assert.IsTrue(bookListViewModel.RemoveCommand.CanExecute(null));
            bookListViewModel.RemoveCommand.Execute(null);
            Assert.IsTrue(entityService.Books.SequenceEqual(new Book[] { fellowship }));
            Assert.AreEqual(fellowship, bookViewModel.Book);
            Assert.IsTrue(bookListView.FirstCellHasFocus);

            // Deselect all Books => the Remove command must be deactivated
            AssertHelper.CanExecuteChangedEvent(bookListViewModel.RemoveCommand, () =>
            {
                bookListViewModel.SelectedBooks.ToList().ForEach(x => bookListViewModel.RemoveSelectedBook(x));
                bookListViewModel.SelectedBook = null;
            });
            Assert.IsFalse(bookListViewModel.RemoveCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddAndRemoveDisableTest()
        {
            Book fellowship = new Book() { Title = "The Fellowship of the Ring" };

            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            
            BookController bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();

            BookListViewModel bookListViewModel = Container.GetExportedValue<BookListViewModel>();
            bookListViewModel.AddSelectedBook(bookListViewModel.Books.Single());
            BookViewModel bookViewModel = Container.GetExportedValue<BookViewModel>();

            var addNewCommand = bookListViewModel.AddNewCommand;
            Assert.IsTrue(addNewCommand.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(addNewCommand, () => bookListViewModel.IsValid = false);
            Assert.IsFalse(addNewCommand.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(addNewCommand, () => bookListViewModel.IsValid = true);
            Assert.IsTrue(addNewCommand.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(addNewCommand, () => bookViewModel.IsValid = false);
            Assert.IsFalse(addNewCommand.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(addNewCommand, () => bookViewModel.IsValid = true);
            Assert.IsTrue(addNewCommand.CanExecute(null));

            var removeCommand = bookListViewModel.RemoveCommand;
            Assert.IsTrue(removeCommand.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(removeCommand, () => bookListViewModel.SelectedBook = null);
            Assert.IsFalse(removeCommand.CanExecute(null));
        }

        [TestMethod]
        public void RemoveAndSelection1Test()
        {
            Book fellowship = new Book() { Title = "The Fellowship of the Ring" };
            Book twoTowers = new Book() { Title = "The Two Towers" };
            Book returnKing = new Book() { Title = "The Return of the King" };

            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            entityService.Books.Add(twoTowers);
            entityService.Books.Add(returnKing);

            BookController bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();

            MockBookListView bookListView = Container.GetExportedValue<MockBookListView>();
            BookListViewModel bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            // Set the sorting to: "The Fell...", "The Retu...", "The Two..."
            bookListViewModel.BookCollectionView = bookListViewModel.Books.OrderBy(b => b.Book.Title);

            // Remove the first book and check that the second one is selected.
            bookListViewModel.SelectedBook = bookListViewModel.Books.Single(b => b.Book == fellowship);
            bookListViewModel.AddSelectedBook(bookListViewModel.SelectedBook);
            bookListViewModel.RemoveCommand.Execute(null);
            Assert.IsTrue(entityService.Books.SequenceEqual(new[] { twoTowers, returnKing }));
            Assert.AreEqual(returnKing, bookListViewModel.SelectedBook.Book);
        }

        [TestMethod]
        public void RemoveAndSelection2Test()
        {
            Book fellowship = new Book() { Title = "The Fellowship of the Ring" };
            Book twoTowers = new Book() { Title = "The Two Towers" };
            Book returnKing = new Book() { Title = "The Return of the King" };

            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            entityService.Books.Add(twoTowers);
            entityService.Books.Add(returnKing);

            BookController bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();

            MockBookListView bookListView = Container.GetExportedValue<MockBookListView>();
            BookListViewModel bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            // Set the sorting to: "The Fell...", "The Retu...", "The Two..."
            bookListViewModel.BookCollectionView = bookListViewModel.Books.OrderBy(b => b.Book.Title);

            // Remove the last book and check that the last one is selected again.
            bookListViewModel.SelectedBook = bookListViewModel.Books.Single(b => b.Book == twoTowers);
            bookListViewModel.AddSelectedBook(bookListViewModel.SelectedBook);
            bookListViewModel.RemoveCommand.Execute(null);
            Assert.IsTrue(entityService.Books.SequenceEqual(new[] { fellowship, returnKing }));
            Assert.AreEqual(returnKing, bookListViewModel.SelectedBook.Book);
        }

        [TestMethod]
        public void RemoveAndSelection3Test()
        {
            Book fellowship = new Book() { Title = "The Fellowship of the Ring" };
            Book twoTowers = new Book() { Title = "The Two Towers" };
            Book returnKing = new Book() { Title = "The Return of the King" };

            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            entityService.Books.Add(twoTowers);
            entityService.Books.Add(returnKing);

            BookController bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();

            MockBookListView bookListView = Container.GetExportedValue<MockBookListView>();
            BookListViewModel bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            bookListViewModel.BookCollectionView = bookListViewModel.Books;

            // Remove all books and check that nothing is selected anymore
            bookListViewModel.SelectedBook = bookListViewModel.Books.Single(b => b.Book == fellowship);
            bookListViewModel.AddSelectedBook(bookListViewModel.SelectedBook);
            bookListViewModel.AddSelectedBook(bookListViewModel.Books.Single(b => b.Book == twoTowers));
            bookListViewModel.AddSelectedBook(bookListViewModel.Books.Single(b => b.Book == returnKing));
            bookListViewModel.RemoveCommand.Execute(null);
            Assert.IsFalse(entityService.Books.Any());
            Assert.IsNull(bookListViewModel.SelectedBook);
        }

        [TestMethod]
        public void LendToTest()
        {
            Book fellowship = new Book() { Title = "The Fellowship of the Ring" };
            Book twoTowers = new Book() { Title = "The Two Towers" };
            Person harry = new Person() { Firstname = "Harry" };
            Person ron = new Person() { Firstname = "Ron" };

            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            entityService.Books.Add(twoTowers);
            entityService.Persons.Add(harry);
            entityService.Persons.Add(ron);

            ShellService shellService = Container.GetExportedValue<ShellService>();
            shellService.ShellView = Container.GetExportedValue<IShellView>();

            BookController bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();

            MockBookListView bookListView = Container.GetExportedValue<MockBookListView>();
            BookListViewModel bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            MockBookView bookView = Container.GetExportedValue<MockBookView>();
            BookViewModel bookViewModel = ViewHelper.GetViewModel<BookViewModel>(bookView);

            // Check that the LendTo Button is enabled
            Assert.IsNull(fellowship.LendTo);
            Assert.AreEqual(fellowship, bookViewModel.Book);
            Assert.IsTrue(bookViewModel.IsEnabled);
            
            // Open the LendTo dialog
            MockLendToView.ShowDialogAction = view =>
            {
                Assert.AreEqual(Container.GetExportedValue<IShellView>(), view.Owner);
                Assert.IsTrue(view.IsVisible);
                LendToViewModel viewModel = (LendToViewModel)view.DataContext;
                Assert.AreEqual(fellowship, viewModel.Book);
                Assert.AreEqual(entityService.Persons, viewModel.Persons);

                // Lend the book to Ron
                viewModel.SelectedPerson = ron;
                viewModel.OkCommand.Execute(null);
            };
            bookViewModel.LendToCommand.Execute(fellowship);
            Assert.AreEqual(ron, fellowship.LendTo);

            // Check that the LendTo Button is disabled when no book is selected anymore.
            AssertHelper.CanExecuteChangedEvent(bookViewModel.LendToCommand, () =>
                bookListViewModel.SelectedBook = null);
            Assert.IsNull(bookViewModel.Book);
            Assert.IsFalse(bookViewModel.IsEnabled);

            MockLendToView.ShowDialogAction = null;
        }
    }
}
