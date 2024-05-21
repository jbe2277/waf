using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Controllers;

/// <summary>Responsible for the book management and the master / detail views.</summary>
[Export]
internal class BookController
{
    private readonly IShellService shellService;
    private readonly IEntityService entityService;
    private readonly BookListViewModel bookListViewModel;
    private readonly BookViewModel bookViewModel;
    private readonly ExportFactory<LendToViewModel> lendToViewModelFactory;
    private readonly DelegateCommand addNewCommand;
    private readonly DelegateCommand removeCommand;
    private readonly DelegateCommand lendToCommand;
    private SynchronizingList<BookDataModel, Book>? bookDataModels;

    [ImportingConstructor]
    public BookController(IShellService shellService, IEntityService entityService, BookListViewModel bookListViewModel, BookViewModel bookViewModel, ExportFactory<LendToViewModel> lendToViewModelFactory)
    {
        this.shellService = shellService;
        this.entityService = entityService;
        this.bookListViewModel = bookListViewModel;
        this.bookViewModel = bookViewModel;
        this.lendToViewModelFactory = lendToViewModelFactory;
        addNewCommand = new(AddNewBook, CanAddNewBook);
        removeCommand = new(RemoveBook, CanRemoveBook);
        lendToCommand = new(p => LendTo((Book)p!));
    }

    internal ObservableListViewCore<BookDataModel>? BooksView { get; private set; }

    public void Initialize()
    {
        bookViewModel.LendToCommand = lendToCommand;
        bookViewModel.PropertyChanged += BookViewModelPropertyChanged;

        bookDataModels = new(entityService.Books, b => new BookDataModel(b, lendToCommand));
        BooksView = new(bookDataModels, null, bookListViewModel.Filter, null);
        bookListViewModel.Books = BooksView;
        bookListViewModel.AddNewCommand = addNewCommand;
        bookListViewModel.RemoveCommand = removeCommand;
        bookListViewModel.PropertyChanged += BookListViewModelPropertyChanged;

        shellService.BookListView = bookListViewModel.View;
        shellService.BookView = bookViewModel.View;

        bookListViewModel.SelectedBook = bookListViewModel.Books.FirstOrDefault();
    }

    private bool CanAddNewBook() => bookListViewModel.IsValid && bookViewModel.IsValid;

    private void AddNewBook()
    {
        var book = new Book();
        book.Validate();
        entityService.Books.Add(book);

        bookListViewModel.SelectedBook = bookDataModels!.Single(b => b.Book == book);
        bookListViewModel.Focus();
    }

    private bool CanRemoveBook() => bookListViewModel.SelectedBook != null;

    private void RemoveBook()
    {
        var booksToExclude = bookListViewModel.SelectedBooks.Except([ bookListViewModel.SelectedBook ]);
        var nextBook = bookListViewModel.Books!.Except(booksToExclude).GetNextElementOrDefault(bookListViewModel.SelectedBook);

        foreach (var x in bookListViewModel.SelectedBooks.ToArray()) entityService.Books.Remove(x.Book);

        bookListViewModel.SelectedBook = nextBook ?? bookListViewModel.Books!.LastOrDefault();
        bookListViewModel.Focus();
    }

    private void LendTo(Book book)
    {
        var lendToViewModel = lendToViewModelFactory.CreateExport().Value;
        lendToViewModel.Book = book;
        lendToViewModel.Persons = entityService.Persons;
        lendToViewModel.SelectedPerson = book.LendTo;
        if (lendToViewModel.ShowDialog(shellService.ShellView!))
        {
            book.LendTo = lendToViewModel.SelectedPerson;
        }
    }

    private void UpdateCommands() => DelegateCommand.RaiseCanExecuteChanged(addNewCommand, removeCommand, lendToCommand);

    private void BookListViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BookListViewModel.SelectedBook))
        {
            bookViewModel.Book = bookListViewModel.SelectedBook?.Book;
            UpdateCommands();
        }
        else if (e.PropertyName == nameof(BookListViewModel.IsValid))
        {
            UpdateCommands();
        }
        else if (e.PropertyName == nameof(BookListViewModel.FilterText))
        {
            BooksView!.Update();
        }
        else if (e.PropertyName == nameof(BookListViewModel.Sort))
        {
            BooksView!.Sort = bookListViewModel.Sort;
        }
    }

    private void BookViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BookViewModel.IsValid)) UpdateCommands();
    }
}
