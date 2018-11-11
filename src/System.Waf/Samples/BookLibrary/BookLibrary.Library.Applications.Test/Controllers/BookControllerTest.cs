using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Waf.Applications;
using System.Waf.UnitTesting;
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
            var entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(new Book() { Title = "The Fellowship of the Ring" });
            entityService.Books.Add(new Book() { Title = "The Two Towers" });
            var bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();

            // Check that Initialize shows the BookListView and BookView
            var shellService = Container.GetExportedValue<ShellService>();
            Assert.IsInstanceOfType(shellService.BookListView, typeof(IBookListView));
            Assert.IsInstanceOfType(shellService.BookView, typeof(IBookView));

            // Check that the first Book is selected
            var bookListView = Container.GetExportedValue<IBookListView>();
            var bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            Assert.AreEqual(entityService.Books.First(), bookListViewModel.SelectedBook.Book);

            // Change the selection
            var bookViewModel = Container.GetExportedValue<BookViewModel>();
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
            AssertHelper.SequenceEqual(new[] { fellowship }, entityService.Books);
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
            var fellowship = new Book() { Title = "The Fellowship of the Ring" };
            var entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            var bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();
            var bookListViewModel = Container.GetExportedValue<BookListViewModel>();
            bookListViewModel.AddSelectedBook(bookListViewModel.Books.Single());
            var bookViewModel = Container.GetExportedValue<BookViewModel>();

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
            var entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            entityService.Books.Add(twoTowers);
            entityService.Books.Add(returnKing);
            var bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();
            var bookListView = Container.GetExportedValue<MockBookListView>();
            var bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            // Set the sorting to: "The Fell...", "The Retu...", "The Two..."
            bookController.BooksView.Sort = x => x.OrderBy(b => b.Book.Title);

            // Remove the first book and check that the second one is selected.
            bookListViewModel.SelectedBook = bookListViewModel.Books.Single(b => b.Book == fellowship);
            bookListViewModel.AddSelectedBook(bookListViewModel.SelectedBook);
            bookListViewModel.RemoveCommand.Execute(null);
            AssertHelper.SequenceEqual(new[] { twoTowers, returnKing }, entityService.Books);
            Assert.AreEqual(returnKing, bookListViewModel.SelectedBook.Book);
        }

        [TestMethod]
        public void RemoveAndSelection2Test()
        {
            var fellowship = new Book() { Title = "The Fellowship of the Ring" };
            var twoTowers = new Book() { Title = "The Two Towers" };
            var returnKing = new Book() { Title = "The Return of the King" };
            var entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            entityService.Books.Add(twoTowers);
            entityService.Books.Add(returnKing);
            var bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();
            var bookListView = Container.GetExportedValue<MockBookListView>();
            var bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            // Set the sorting to: "The Fell...", "The Retu...", "The Two..."
            bookController.BooksView.Sort = x => x.OrderBy(b => b.Book.Title);

            // Remove the last book and check that the last one is selected again.
            bookListViewModel.SelectedBook = bookListViewModel.Books.Single(b => b.Book == twoTowers);
            bookListViewModel.AddSelectedBook(bookListViewModel.SelectedBook);
            bookListViewModel.RemoveCommand.Execute(null);
            AssertHelper.SequenceEqual(new[] { fellowship, returnKing }, entityService.Books);
            Assert.AreEqual(returnKing, bookListViewModel.SelectedBook.Book);
        }

        [TestMethod]
        public void RemoveAndSelection3Test()
        {
            var fellowship = new Book() { Title = "The Fellowship of the Ring" };
            var twoTowers = new Book() { Title = "The Two Towers" };
            var returnKing = new Book() { Title = "The Return of the King" };
            var entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            entityService.Books.Add(twoTowers);
            entityService.Books.Add(returnKing);
            var bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();
            var bookListView = Container.GetExportedValue<MockBookListView>();
            var bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);

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
            var fellowship = new Book() { Title = "The Fellowship of the Ring" };
            var twoTowers = new Book() { Title = "The Two Towers" };
            var harry = new Person() { Firstname = "Harry" };
            var ron = new Person() { Firstname = "Ron" };
            var entityService = Container.GetExportedValue<IEntityService>();
            entityService.Books.Add(fellowship);
            entityService.Books.Add(twoTowers);
            entityService.Persons.Add(harry);
            entityService.Persons.Add(ron);
            var shellService = Container.GetExportedValue<ShellService>();
            shellService.ShellView = Container.GetExportedValue<IShellView>();
            var bookController = Container.GetExportedValue<BookController>();
            bookController.Initialize();
            var bookListView = Container.GetExportedValue<MockBookListView>();
            var bookListViewModel = ViewHelper.GetViewModel<BookListViewModel>(bookListView);
            var bookView = Container.GetExportedValue<MockBookView>();
            var bookViewModel = ViewHelper.GetViewModel<BookViewModel>(bookView);

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
