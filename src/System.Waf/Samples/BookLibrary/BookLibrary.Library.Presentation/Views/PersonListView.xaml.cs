using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;
using Waf.BookLibrary.Library.Presentation.Controls;

namespace Waf.BookLibrary.Library.Presentation.Views
{
    [Export(typeof(IPersonListView))]
    public partial class PersonListView : UserControl, IPersonListView
    {
        private static readonly Dictionary<string, Func<Person, IComparable>> sortSelectors = new Dictionary<string, Func<Person, IComparable>>
        {
            { nameof(Person.Firstname), x => x.Firstname },
            { nameof(Person.Lastname), x => x.Lastname },
            { nameof(Person.Email), x => x.Email }
        };
        private readonly Lazy<PersonListViewModel> viewModel;
        
        public PersonListView()
        {
            InitializeComponent();
            viewModel = new Lazy<PersonListViewModel>(() => ViewHelper.GetViewModel<PersonListViewModel>(this));
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
                ViewModel.Sort = DataGridHelper.GetSorting(firstColumn, sortSelectors);
                personTable.SelectedIndex = 0;
                FocusFirstCell();
            }
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Person person in e.RemovedItems)
            {
                ViewModel.RemoveSelectedPerson(person);
            }
            foreach (Person person in e.AddedItems)
            {
                ViewModel.AddSelectedPerson(person);
            }
        }

        private void DataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            ViewModel.Sort = DataGridHelper.HandleDataGridSorting(e, sortSelectors);
        }

        private void EmailClick(object sender, RoutedEventArgs e)
        {
            var hyperlink = (Hyperlink)e.OriginalSource;
            ViewModel.CreateNewEmailCommand.Execute(hyperlink.DataContext);
        }
    }
}
