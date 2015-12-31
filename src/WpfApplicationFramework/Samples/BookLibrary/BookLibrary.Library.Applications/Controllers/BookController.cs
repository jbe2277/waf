using System.Collections.Generic;
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
        private SynchronizingCollection<BookDataModel, Book> bookDataModels;
        

        [ImportingConstructor]
        public BookController(IShellService shellService, IEntityService entityService,
            BookListViewModel bookListViewModel, BookViewModel bookViewModel, ExportFactory<LendToViewModel> lendToViewModelFactory)
        {
            this.shellService = shellService;
            this.entityService = entityService;
            this.bookListViewModel = bookListViewModel;
            this.bookViewModel = bookViewModel;
            this.lendToViewModelFactory = lendToViewModelFactory;
            this.addNewCommand = new DelegateCommand(AddNewBook, CanAddNewBook);
            this.removeCommand = new DelegateCommand(RemoveBook, CanRemoveBook);
            this.lendToCommand = new DelegateCommand(p => LendTo((Book)p));
        }


        public void Initialize()
        {
            bookViewModel.LendToCommand = lendToCommand;
            PropertyChangedEventManager.AddHandler(bookViewModel, BookViewModelPropertyChanged, "");

            bookDataModels = new SynchronizingCollection<BookDataModel, Book>(entityService.Books, 
                b => new BookDataModel(b, lendToCommand));
            bookListViewModel.Books = bookDataModels;
            bookListViewModel.AddNewCommand = addNewCommand;
            bookListViewModel.RemoveCommand = removeCommand;
            PropertyChangedEventManager.AddHandler(bookListViewModel, BookListViewModelPropertyChanged, "");

            shellService.BookListView = bookListViewModel.View;
            shellService.BookView = bookViewModel.View;

            bookListViewModel.SelectedBook = bookListViewModel.Books.FirstOrDefault();
        }

        private bool CanAddNewBook() { return bookListViewModel.IsValid && bookViewModel.IsValid; }

        private void AddNewBook()
        {
            Book book = new Book();
            book.Validate();
            entityService.Books.Add(book);

            bookListViewModel.SelectedBook = bookDataModels.Single(b => b.Book == book);
            bookListViewModel.Focus();
        }

        private bool CanRemoveBook() 
        { 
            // Unfortunately, it is necessary to deactivate the Remove command when a cell is invalid in the DataGrid. Otherwise, it might freeze.
            // See: https://connect.microsoft.com/VisualStudio/feedback/details/777761/wpf-datagrid-becomes-readonly-after-deleting-an-invalid-row
            return bookListViewModel.SelectedBook != null && bookListViewModel.IsValid && bookViewModel.IsValid; 
        }

        private void RemoveBook()
        {
            // Use the BookCollectionView, which represents the sorted/filtered state of the books, to determine the next book to select.
            IEnumerable<BookDataModel> booksToExclude = bookListViewModel.SelectedBooks.Except(new[] { bookListViewModel.SelectedBook });
            BookDataModel nextBook = CollectionHelper.GetNextElementOrDefault(bookListViewModel.BookCollectionView.Except(booksToExclude), 
                bookListViewModel.SelectedBook);

            foreach (BookDataModel book in bookListViewModel.SelectedBooks.ToArray())
            {
                entityService.Books.Remove(book.Book);
            }

            bookListViewModel.SelectedBook = nextBook ?? bookListViewModel.BookCollectionView.LastOrDefault();
            bookListViewModel.Focus();
        }

        private void LendTo(Book book)
        {
            LendToViewModel lendToViewModel = lendToViewModelFactory.CreateExport().Value;
            lendToViewModel.Book = book;
            lendToViewModel.Persons = entityService.Persons;
            if (lendToViewModel.ShowDialog(shellService.ShellView))
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

        private void BookListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
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
        }

        private void BookViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BookViewModel.IsValid))
            {
                UpdateCommands();
            }
        }
    }
}
