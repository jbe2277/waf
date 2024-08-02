using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.BookLibrary.Library.Applications.Services;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Integration;

[TestClass]
public class IntegrationTest : ApplicationsTest
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        var dbContextService = Get<MockDBContextService>();
        dbContextService.ContextCreated = x => 
        {
            var harry = new Person { Firstname = "Harry", Lastname = "Potter" };
            var ron = new Person { Firstname = "Ron", Lastname = "Weasley" };
            x.Set<Person>().Add(harry);
            x.Set<Person>().Add(ron);
            x.Set<Book>().Add(new Book { Title = "The Lord of the Rings - The Fellowship of the Ring", Author = "J.R.R. Tolkien", LendTo = harry });
            x.Set<Book>().Add(new Book { Title = "Star Wars - Heir to the Empire", Author = "Timothy Zahn", LendTo = ron });
            x.Set<Book>().Add(new Book { Title = "Serenity, Vol 1: Those Left Behind", Author = "Joss Whedon, Brett Matthews, Will Conrad" });
        };
    }

    [TestMethod]
    public void SortFilterAndChangeLendToStatusTest()
    {
        var moduleController = Get<ModuleController>();
        moduleController.Initialize();
        moduleController.Run();

        var shellView = Get<MockShellView>();
        Assert.IsTrue(shellView.IsVisible);
        var bookListView = (MockBookListView)shellView.ViewModel.ShellService.BookListView!;        
        var bookView = (MockBookView)shellView.ViewModel.ShellService.BookView!;

        var books = bookListView.ViewModel.Books!;
        bookListView.SingleSelect(books[0]);
        
        // Sort Book list by Title
        Assert.IsTrue(books[0].Book.Title.StartsWith("The Lord"));
        bookListView.ViewModel.Sort = x => x.OrderBy(y => y.Book.Title);
        Assert.IsTrue(books[0].Book.Title.StartsWith("Serenity"));

        // Filter Book list by search text
        bookListView.ViewModel.FilterText = "Star";
        var book = books.Single().Book;
        Assert.IsTrue(book.Title.StartsWith("Star"));
        bookListView.SingleSelect(books[0]);

        Assert.AreSame(book, bookView.ViewModel.Book);

        // Check that the Star Wars book was lend to Ron. Then change the status to not lend to anyone.
        var ron = book.LendTo!;
        Assert.AreEqual("Ron", ron.Firstname);
        MockLendToView.ShowDialogAction = v =>
        {
            Assert.AreSame(book, v.ViewModel.Book);
            Assert.AreSame(ron, v.ViewModel.SelectedPerson);
            Assert.IsTrue(v.ViewModel.IsLendTo);
            v.ViewModel.IsLendTo = false;
            v.ViewModel.OkCommand.Execute(null);
        };
        bookView.ViewModel.LendToCommand!.Execute(book);
        Assert.IsNull(book.LendTo);

        // Check that The Lord of the Rings book was lend to Harry.
        bookListView.ViewModel.FilterText = "";
        var book2 = books.Select(x => x.Book).First(x => x.Title.StartsWith("The Lord"));
        var harry = book2.LendTo!;
        bookListView.SingleSelect(books.Single(x => x.Book == book2));
        Assert.AreEqual("Harry", bookView.ViewModel.Book?.LendTo?.Firstname);

        // Switch to Persons(List)View and select Harry
        var personListView = (MockPersonListView)shellView.ViewModel.ShellService.PersonListView!;
        var personView = (MockPersonView)shellView.ViewModel.ShellService?.PersonView!;
        var persons = personListView.ViewModel.Persons!;
        Assert.AreEqual(2, persons.Count);
        Assert.AreSame(harry, persons.Single(x => x.Firstname == "Harry"));
        personListView.SingleSelect(harry);
        Assert.AreSame(harry, personView.ViewModel.Person);
        
        // Select all Persons and remove all of them
        foreach (var x in persons.Where(x => x != harry)) personListView.ViewModel.AddSelectedPerson(x);
        personListView.ViewModel.RemoveCommand!.Execute(null);
        Assert.AreEqual(0, persons.Count);

        // Check that The Lord of the Rings book is not lend to anyone.
        Assert.IsNull(book2.LendTo);

        // Use a search string which does not exist -> Books list is empty now
        bookListView.ViewModel.FilterText = "Star Trek";
        Assert.AreEqual(0, books.Count);
        bookListView.SingleSelect(null);
        Assert.IsNull(bookView.ViewModel.Book);

        moduleController.Shutdown();
    }
}
