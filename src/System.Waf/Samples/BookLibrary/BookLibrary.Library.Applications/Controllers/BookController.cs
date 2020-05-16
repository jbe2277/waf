using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Foundation;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Controllers
{
    /// <summary>
    /// Responsible for the book management and the master / detail views.
    /// </summary>
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
        private SynchronizingCollection<BookDataModel, Book>? bookDataModels;

        [ImportingConstructor]
        public BookController(IShellService shellService, IEntityService entityService,
            BookListViewModel bookListViewModel, BookViewModel bookViewModel, ExportFactory<LendToViewModel> lendToViewModelFactory)
        {
            this.shellService = shellService;
            this.entityService = entityService;
            this.bookListViewModel = bookListViewModel;
            this.bookViewModel = bookViewModel;
            this.lendToViewModelFactory = lendToViewModelFactory;
            addNewCommand = new DelegateCommand(AddNewBook, CanAddNewBook);
            removeCommand = new DelegateCommand(RemoveBook, CanRemoveBook);
            lendToCommand = new DelegateCommand(p => LendTo((Book)p!));
        }

        internal ObservableListView<BookDataModel>? BooksView { get; private set; }

        public void Initialize()
        {
            bookViewModel.LendToCommand = lendToCommand;
            bookViewModel.PropertyChanged += BookViewModelPropertyChanged;

            bookDataModels = new SynchronizingCollection<BookDataModel, Book>(entityService.Books, 
                b => new BookDataModel(b, lendToCommand));
            BooksView = new ObservableListView<BookDataModel>(bookDataModels, null, bookListViewModel.Filter, null);
            bookListViewModel.Books = BooksView;
            bookListViewModel.AddNewCommand = addNewCommand;
            bookListViewModel.RemoveCommand = removeCommand;
            bookListViewModel.PropertyChanged += BookListViewModelPropertyChanged;

            shellService.BookListView = bookListViewModel.View;
            shellService.BookView = bookViewModel.View;

            bookListViewModel.SelectedBook = bookListViewModel.Books.FirstOrDefault();
        }

        private bool CanAddNewBook() { return bookListViewModel.IsValid && bookViewModel.IsValid; }

        private void AddNewBook()
        {
            var book = new Book();
            book.Validate();
            entityService.Books.Add(book);

            bookListViewModel.SelectedBook = bookDataModels.Single(b => b.Book == book);
            bookListViewModel.Focus();
        }

        private bool CanRemoveBook() 
        {
            return bookListViewModel.SelectedBook != null;
        }

        private void RemoveBook()
        {
            var booksToExclude = bookListViewModel.SelectedBooks.Except(new[] { bookListViewModel.SelectedBook });
            var nextBook = bookListViewModel.Books.Except(booksToExclude).GetNextElementOrDefault(bookListViewModel.SelectedBook);

            foreach (BookDataModel book in bookListViewModel.SelectedBooks.ToArray())
            {
                entityService.Books.Remove(book.Book);
            }

            bookListViewModel.SelectedBook = nextBook ?? bookListViewModel.Books.LastOrDefault();
            bookListViewModel.Focus();
        }

        private void LendTo(Book book)
        {
            var lendToViewModel = lendToViewModelFactory.CreateExport().Value;
            lendToViewModel.Book = book;
            lendToViewModel.Persons = entityService.Persons;
            lendToViewModel.SelectedPerson = entityService.Persons.FirstOrDefault();
            if (lendToViewModel.ShowDialog(shellService.ShellView!))
            {
                book.LendTo = lendToViewModel.SelectedPerson;
            }
        }

        private void UpdateCommands()
        {
            addNewCommand.RaiseCanExecuteChanged();
            removeCommand.RaiseCanExecuteChanged();
            lendToCommand.RaiseCanExecuteChanged();
        }

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
            if (e.PropertyName == nameof(BookViewModel.IsValid))
            {
                UpdateCommands();
            }
        }
    }
}
