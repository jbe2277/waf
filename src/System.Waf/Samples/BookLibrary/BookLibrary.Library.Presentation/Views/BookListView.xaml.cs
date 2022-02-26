using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Presentation.Controls;
using System.Windows;
using System.Windows.Controls;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Presentation.Views;

[Export(typeof(IBookListView))]
public partial class BookListView : IBookListView
{
    private readonly Lazy<BookListViewModel> viewModel;

    public BookListView()
    {
        InitializeComponent();
        viewModel = new Lazy<BookListViewModel>(() => this.GetViewModel<BookListViewModel>()!);
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
            ViewModel.Sort = DataGridHelper.GetSorting<BookDataModel>(firstColumn);
            bookTable.SelectedIndex = 0;
            FocusFirstCell();
        }
    }

    private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (BookDataModel book in e.RemovedItems) ViewModel.RemoveSelectedBook(book);
        foreach (BookDataModel book in e.AddedItems) ViewModel.AddSelectedBook(book);
    }

    private void DataGridSorting(object sender, DataGridSortingEventArgs e) => ViewModel.Sort = DataGridHelper.HandleDataGridSorting<BookDataModel>(e);
}
