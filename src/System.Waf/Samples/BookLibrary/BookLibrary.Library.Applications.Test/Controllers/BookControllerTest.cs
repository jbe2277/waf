using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Test.BookLibrary.Library.Applications.Services;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Controllers;

[TestClass]
public class BookControllerTest : ApplicationsTest
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        Get<EntityService>().BookLibraryContext = Get<MockDBContextService>().GetBookLibraryContext(out _);
    }

    protected override void OnCleanup()
    {
        MockLendToView.ShowDialogAction = null;
        base.OnCleanup();
    }

    [TestMethod]
    public void SelectionTest()
    {
        var entityService = Get<IEntityService>();
        entityService.Books.Add(new() { Title = "The Fellowship of the Ring" });
        entityService.Books.Add(new() { Title = "The Two Towers" });
        var bookController = Get<BookController>();
        bookController.Initialize();
        bookController.Run();

        // Check that Initialize shows the BookListView and BookView
        var shellService = Get<ShellService>();
        Assert.IsInstanceOfType(shellService.BookListView, typeof(IBookListView));
        Assert.IsInstanceOfType(shellService.BookView, typeof(IBookView));

        // Check that the first Book is selected
        var bookListView = Get<MockBookListView>();
        var bookListViewModel = bookListView.ViewModel;
        Assert.AreEqual(entityService.Books[0], bookListViewModel.SelectedBook?.Book);

        // Change the selection
        var bookViewModel = Get<BookViewModel>();
        bookListViewModel.SelectedBook = bookListViewModel.Books![^1];
        Assert.AreEqual(entityService.Books[^1], bookViewModel.Book);
    }

    [TestMethod]
    public void AddAndRemoveTest()
    {
        var fellowship = new Book() { Title = "The Fellowship of the Ring" };
        var twoTowers = new Book() { Title = "The Two Towers" };
        var entityService = Get<IEntityService>();
        entityService.Books.Add(fellowship);
        entityService.Books.Add(twoTowers);
        var bookController = Get<BookController>();
        bookController.Initialize();
        bookController.Run();
        var bookListView = Get<MockBookListView>();
        var bookListViewModel = bookListView.ViewModel;
        var bookView = Get<MockBookView>();
        var bookViewModel = bookView.ViewModel;

        // Add a new Book
        Assert.AreEqual(2, entityService.Books.Count);
        Assert.IsTrue(bookListViewModel.AddNewCommand!.CanExecute(null));
        bookListViewModel.AddNewCommand.Execute(null);
        Assert.AreEqual(3, entityService.Books.Count);

        // Check that the new Book is selected and the first control gets the focus
        Assert.AreEqual(entityService.Books[^1], bookViewModel.Book);
        Assert.IsTrue(bookListView.FirstCellHasFocus);

        // Simulate an invalid UI input state => the user can't add more books
        AssertHelper.CanExecuteChangedEvent(bookListViewModel.AddNewCommand, () => bookViewModel.IsValid = false);
        Assert.IsFalse(bookListViewModel.AddNewCommand.CanExecute(null));

        // Remove the last two Books at once and check that the last remaining book is selected.
        bookViewModel.IsValid = true;
        bookListView.FirstCellHasFocus = false;
        bookListViewModel.AddSelectedBook(bookListViewModel.Books!.Single(b => b.Book == twoTowers));
        bookListViewModel.AddSelectedBook(bookListViewModel.Books![^1]);
        Assert.IsTrue(bookListViewModel.RemoveCommand!.CanExecute(null));
        bookListViewModel.RemoveCommand.Execute(null);
        AssertHelper.SequenceEqual([ fellowship ], entityService.Books);
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
        var entityService = Get<IEntityService>();
        entityService.Books.Add(fellowship);
        var bookController = Get<BookController>();
        bookController.Initialize();
        bookController.Run();
        var bookListViewModel = Get<BookListViewModel>();
        bookListViewModel.AddSelectedBook(bookListViewModel.Books!.Single());
        var bookViewModel = Get<BookViewModel>();

        var addNewCommand = bookListViewModel.AddNewCommand!;
        Assert.IsTrue(addNewCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(addNewCommand, () => bookListViewModel.IsValid = false);
        Assert.IsFalse(addNewCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(addNewCommand, () => bookListViewModel.IsValid = true);
        Assert.IsTrue(addNewCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(addNewCommand, () => bookViewModel.IsValid = false);
        Assert.IsFalse(addNewCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(addNewCommand, () => bookViewModel.IsValid = true);
        Assert.IsTrue(addNewCommand.CanExecute(null));

        var removeCommand = bookListViewModel.RemoveCommand!;
        Assert.IsTrue(removeCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(removeCommand, () => bookListViewModel.SelectedBook = null);
        Assert.IsFalse(removeCommand.CanExecute(null));
    }

    [TestMethod]
    public void RemoveAndSelection1Test()
    {
        var fellowship = new Book() { Title = "The Fellowship of the Ring" };
        var twoTowers = new Book() { Title = "The Two Towers" };
        var returnKing = new Book() { Title = "The Return of the King" };
        var entityService = Get<IEntityService>();
        entityService.Books.Add(fellowship);
        entityService.Books.Add(twoTowers);
        entityService.Books.Add(returnKing);
        var bookController = Get<BookController>();
        bookController.Initialize();
        bookController.Run();
        var bookListView = Get<MockBookListView>();
        var bookListViewModel = bookListView.ViewModel;
        // Set the sorting to: "The Fell...", "The Retu...", "The Two..."
        bookController.BooksView!.Sort = x => x.OrderBy(b => b.Book.Title);

        // Remove the first book and check that the second one is selected.
        bookListViewModel.SelectedBook = bookListViewModel.Books!.Single(b => b.Book == fellowship);
        bookListViewModel.AddSelectedBook(bookListViewModel.SelectedBook);
        bookListViewModel.RemoveCommand!.Execute(null);
        AssertHelper.SequenceEqual([ twoTowers, returnKing ], entityService.Books);
        Assert.AreEqual(returnKing, bookListViewModel.SelectedBook.Book);
    }

    [TestMethod]
    public void RemoveAndSelection2Test()
    {
        var fellowship = new Book() { Title = "The Fellowship of the Ring" };
        var twoTowers = new Book() { Title = "The Two Towers" };
        var returnKing = new Book() { Title = "The Return of the King" };
        var entityService = Get<IEntityService>();
        entityService.Books.Add(fellowship);
        entityService.Books.Add(twoTowers);
        entityService.Books.Add(returnKing);
        var bookController = Get<BookController>();
        bookController.Initialize();
        bookController.Run();
        var bookListView = Get<MockBookListView>();
        var bookListViewModel = bookListView.ViewModel;
        // Set the sorting to: "The Fell...", "The Retu...", "The Two..."
        bookController.BooksView!.Sort = x => x.OrderBy(b => b.Book.Title);

        // Remove the last book and check that the last one is selected again.
        bookListViewModel.SelectedBook = bookListViewModel.Books!.Single(b => b.Book == twoTowers);
        bookListViewModel.AddSelectedBook(bookListViewModel.SelectedBook);
        bookListViewModel.RemoveCommand!.Execute(null);
        AssertHelper.SequenceEqual([ fellowship, returnKing ], entityService.Books);
        Assert.AreEqual(returnKing, bookListViewModel.SelectedBook.Book);
    }

    [TestMethod]
    public void RemoveAndSelection3Test()
    {
        var fellowship = new Book() { Title = "The Fellowship of the Ring" };
        var twoTowers = new Book() { Title = "The Two Towers" };
        var returnKing = new Book() { Title = "The Return of the King" };
        var entityService = Get<IEntityService>();
        entityService.Books.Add(fellowship);
        entityService.Books.Add(twoTowers);
        entityService.Books.Add(returnKing);
        var bookController = Get<BookController>();
        bookController.Initialize();
        bookController.Run();
        var bookListView = Get<MockBookListView>();
        var bookListViewModel = bookListView.ViewModel;

        // Remove all books and check that nothing is selected anymore
        bookListViewModel.SelectedBook = bookListViewModel.Books!.Single(b => b.Book == fellowship);
        bookListViewModel.AddSelectedBook(bookListViewModel.SelectedBook);
        bookListViewModel.AddSelectedBook(bookListViewModel.Books!.Single(b => b.Book == twoTowers));
        bookListViewModel.AddSelectedBook(bookListViewModel.Books!.Single(b => b.Book == returnKing));
        bookListViewModel.RemoveCommand!.Execute(null);
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
        var entityService = Get<IEntityService>();
        entityService.Books.Add(fellowship);
        entityService.Books.Add(twoTowers);
        entityService.Persons.Add(harry);
        entityService.Persons.Add(ron);
        var shellService = Get<ShellService>();
        shellService.ShellView = Get<IShellView>();
        var bookController = Get<BookController>();
        bookController.Initialize();
        bookController.Run();
        var bookListView = Get<MockBookListView>();
        var bookListViewModel = bookListView.ViewModel;
        var bookView = Get<MockBookView>();
        var bookViewModel = bookView.ViewModel;

        // Check that the LendTo Button is enabled
        Assert.IsNull(fellowship.LendTo);
        Assert.AreEqual(fellowship, bookViewModel.Book);
        Assert.IsTrue(bookViewModel.IsEnabled);

        // Open the LendTo dialog
        MockLendToView.ShowDialogAction = view =>
        {
            Assert.AreEqual(Get<IShellView>(), view.Owner);
            Assert.IsTrue(view.IsVisible);
            LendToViewModel viewModel = view.ViewModel;
            Assert.AreEqual(fellowship, viewModel.Book);
            Assert.AreEqual(entityService.Persons, viewModel.Persons);
            Assert.IsFalse(viewModel.IsLendTo);
            viewModel.IsLendTo = true;

            // Lend the book to Ron
            viewModel.SelectedPerson = ron;
            viewModel.OkCommand.Execute(null);
        };
        bookViewModel.LendToCommand!.Execute(fellowship);
        Assert.AreEqual(ron, fellowship.LendTo);

        // Check that the LendTo Button is disabled when no book is selected anymore.
        AssertHelper.CanExecuteChangedEvent(bookViewModel.LendToCommand, () => bookListViewModel.SelectedBook = null);
        Assert.IsNull(bookViewModel.Book);
        Assert.IsFalse(bookViewModel.IsEnabled);
    }
}
