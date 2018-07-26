using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;
using Waf.BookLibrary.Library.Presentation.Controls;

namespace Waf.BookLibrary.Library.Presentation.Views
{
    [Export(typeof(IBookListView))]
    public partial class BookListView : UserControl, IBookListView
    {
        private static readonly Dictionary<string, Func<BookDataModel, IComparable>> sortSelectors = new Dictionary<string, Func<BookDataModel, IComparable>>
        {
            { nameof(BookDataModel.Book) + "." + nameof(Book.Title), x => x.Book.Title },
            { nameof(BookDataModel.Book) + "." + nameof(Book.Author), x => x.Book.Author },
            { nameof(BookDataModel.Book) + "." + nameof(Book.PublishDate), x => x.Book.PublishDate },
            { nameof(BookDataModel.Book) + "." + nameof(Book.LendTo) + "." + nameof(Person.Firstname), x => x.Book.LendTo?.Firstname }
        };
        private readonly Lazy<BookListViewModel> viewModel;

        public BookListView()
        {
            InitializeComponent();
            viewModel = new Lazy<BookListViewModel>(() => ViewHelper.GetViewModel<BookListViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }

        private BookListViewModel ViewModel => viewModel.Value;

        public void FocusFirstCell()
        {
            bookTable.Focus();
            bookTable.CurrentCell = new DataGridCellInfo(bookTable.SelectedItem, bookTable.Columns[0]);
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            Loaded -= FirstTimeLoadedHandler;  // Ensure that this handler is called only once.
            if (bookTable.Items.Count > 0)
            {
                var firstColumn = bookTable.ColumnFromDisplayIndex(0);
                firstColumn.SortDirection = ListSortDirection.Ascending;
                ViewModel.Sort = DataGridHelper.GetSorting(firstColumn, sortSelectors);
                bookTable.SelectedIndex = 0;
                FocusFirstCell();
            }
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (BookDataModel book in e.RemovedItems)
            {
                ViewModel.RemoveSelectedBook(book);
            }
            foreach (BookDataModel book in e.AddedItems)
            {
                ViewModel.AddSelectedBook(book);
            }
        }

        private void DataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            ViewModel.Sort = DataGridHelper.HandleDataGridSorting(e, sortSelectors);
        }
    }
}
