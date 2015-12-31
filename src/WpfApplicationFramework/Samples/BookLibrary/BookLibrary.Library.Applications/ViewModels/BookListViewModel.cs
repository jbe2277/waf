using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
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
        private IReadOnlyList<BookDataModel> books;
        private BookDataModel selectedBook;
        private ICommand addNewCommand;
        private ICommand removeCommand;
        private string filterText = "";

        
        [ImportingConstructor]
        public BookListViewModel(IBookListView view) : base(view)
        {
            selectedBooks = new ObservableCollection<BookDataModel>();
        }


        public IReadOnlyList<BookDataModel> SelectedBooks => selectedBooks;

        public IEnumerable<BookDataModel> BookCollectionView { get; set; }

        public bool IsValid
        {
            get { return isValid; }
            set { SetProperty(ref isValid, value); }
        }

        public IReadOnlyList<BookDataModel> Books
        {
            get { return books; }
            set { SetProperty(ref books, value); }
        }

        public BookDataModel SelectedBook
        {
            get { return selectedBook; }
            set { SetProperty(ref selectedBook, value); }
        }

        public ICommand AddNewCommand
        {
            get { return addNewCommand; }
            set { SetProperty(ref addNewCommand, value); }
        }

        public ICommand RemoveCommand
        {
            get { return removeCommand; }
            set { SetProperty(ref removeCommand, value); }
        }

        public string FilterText
        {
            get { return filterText; }
            set { SetProperty(ref filterText, value); }
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
