using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels
{
    [Export]
    public class BookListViewModel : ViewModel<IBookListView>
    {
        private readonly ObservableCollection<BookDataModel> selectedBooks;
        private bool isValid = true;
        private BookDataModel selectedBook;
        private string filterText = "";
        private Func<IEnumerable<BookDataModel>, IOrderedEnumerable<BookDataModel>> sort;

        [ImportingConstructor]
        public BookListViewModel(IBookListView view) : base(view)
        {
            selectedBooks = new ObservableCollection<BookDataModel>();
        }

        public IReadOnlyList<BookDataModel> SelectedBooks => selectedBooks;

        public bool IsValid
        {
            get { return isValid; }
            set { SetProperty(ref isValid, value); }
        }

        public IReadOnlyList<BookDataModel> Books { get; set; }

        public BookDataModel SelectedBook
        {
            get { return selectedBook; }
            set { SetProperty(ref selectedBook, value); }
        }

        public ICommand AddNewCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public string FilterText
        {
            get { return filterText; }
            set { SetProperty(ref filterText, value); }
        }

        public Func<IEnumerable<BookDataModel>, IOrderedEnumerable<BookDataModel>> Sort
        {
            get { return sort; }
            set { SetProperty(ref sort, value); }
        }

        public void Focus()
        {
            ViewCore.FocusFirstCell();
        }

        public bool Filter(BookDataModel bookDataModel)
        {
            if (string.IsNullOrEmpty(filterText)) { return true; }
            
            Book book = bookDataModel.Book;
            return string.IsNullOrEmpty(book.Title) || book.Title.IndexOf(filterText, StringComparison.CurrentCultureIgnoreCase) >= 0
                || string.IsNullOrEmpty(book.Author) || book.Author.IndexOf(filterText, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public void AddSelectedBook(BookDataModel bookDataModel)
        {
            selectedBooks.Add(bookDataModel);
        }

        public void RemoveSelectedBook(BookDataModel bookDataModel)
        {
            selectedBooks.Remove(bookDataModel);
        }
    }
}
