using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Presentation.Views
{
    [Export(typeof(IBookListView))]
    public partial class BookListView : UserControl, IBookListView
    {
        private readonly Lazy<BookListViewModel> viewModel;
        private ICollectionView bookCollectionView;

        
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
            // Ensure that this handler is called only once.
            Loaded -= FirstTimeLoadedHandler;
            
            // The following code doesn't work in the WPF Designer environment (Cider or Blend).
            if (!WafConfiguration.IsInDesignMode)
            {
                bookCollectionView = CollectionViewSource.GetDefaultView(ViewModel.Books);
                bookCollectionView.Filter = Filter;
                ViewModel.BookCollectionView = bookCollectionView.Cast<BookDataModel>();

                bookTable.Focus();
                bookTable.CurrentCell = new DataGridCellInfo(ViewModel.Books.FirstOrDefault(), bookTable.Columns[0]);
            }
        }

        private void FilterBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            // Refresh must not be called as long the DataGrid is in edit mode.
            if (bookTable.CommitEdit(DataGridEditingUnit.Row, true))
            {
                bookCollectionView.Refresh();
            }
        }
        
        private bool Filter(object obj)
        {
            return ViewModel.Filter((BookDataModel)obj);
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
    }
}
