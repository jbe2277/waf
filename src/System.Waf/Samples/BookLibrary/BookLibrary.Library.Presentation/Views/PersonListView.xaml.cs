using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Presentation.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Presentation.Views
{
    [Export(typeof(IPersonListView))]
    public partial class PersonListView : IPersonListView
    {
        private readonly Lazy<PersonListViewModel> viewModel;
        
        public PersonListView()
        {
            InitializeComponent();
            viewModel = new Lazy<PersonListViewModel>(() => this.GetViewModel<PersonListViewModel>()!);
            Loaded += FirstTimeLoadedHandler;
        }

        private PersonListViewModel ViewModel => viewModel.Value;

        public void FocusFirstCell()
        {
            personTable.Focus();
            personTable.CurrentCell = new DataGridCellInfo(personTable.SelectedItem, personTable.Columns[0]);
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            Loaded -= FirstTimeLoadedHandler;  // Ensure that this handler is called only once.
            if (personTable.Items.Count > 0)
            {
                var firstColumn = personTable.ColumnFromDisplayIndex(0);
                firstColumn.SortDirection = ListSortDirection.Ascending;
                ViewModel.Sort = DataGridHelper.GetSorting<Person>(firstColumn);
                personTable.SelectedIndex = 0;
                FocusFirstCell();
            }
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Person? person in e.RemovedItems)
            {
                ViewModel.RemoveSelectedPerson(person!);
            }
            foreach (Person? person in e.AddedItems)
            {
                ViewModel.AddSelectedPerson(person!);
            }
        }

        private void DataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            ViewModel.Sort = DataGridHelper.HandleDataGridSorting<Person>(e);
        }

        private void EmailClick(object sender, RoutedEventArgs e)
        {
            var hyperlink = (Hyperlink)e.OriginalSource;
            ViewModel.CreateNewEmailCommand!.Execute(hyperlink.DataContext);
        }
    }
}
