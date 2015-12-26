using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;
using System.Waf;

namespace Waf.BookLibrary.Library.Presentation.Views
{
    [Export(typeof(IPersonListView))]
    public partial class PersonListView : UserControl, IPersonListView
    {
        private readonly Lazy<PersonListViewModel> viewModel;
        private ICollectionView personCollectionView;
        
        
        public PersonListView()
        {
            InitializeComponent();

            viewModel = new Lazy<PersonListViewModel>(() => ViewHelper.GetViewModel<PersonListViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }


        private PersonListViewModel ViewModel { get { return viewModel.Value; } }


        public void FocusFirstCell()
        {
            personTable.Focus();
            personTable.CurrentCell = new DataGridCellInfo(personTable.SelectedItem, personTable.Columns[0]);
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            // Ensure that this handler is called only once.
            Loaded -= FirstTimeLoadedHandler;
            
            // The following code doesn't work in the WPF Designer environment (Cider or Blend).
            if (!WafConfiguration.IsInDesignMode)
            {
                personCollectionView = CollectionViewSource.GetDefaultView(ViewModel.Persons);
                personCollectionView.Filter = Filter;
                ViewModel.PersonCollectionView = personCollectionView.Cast<Person>();

                personTable.Focus();
                personTable.CurrentCell = new DataGridCellInfo(ViewModel.Persons.FirstOrDefault(), personTable.Columns[0]);
            }
        }

        private void FilterBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            // Refresh must not be called as long the DataGrid is in edit mode.
            if (personTable.CommitEdit(DataGridEditingUnit.Row, true))
            {
                personCollectionView.Refresh();
            }
        }

        private bool Filter(object obj)
        {
            return ViewModel.Filter((Person)obj);
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

        private void EmailClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = (Hyperlink)e.OriginalSource;
            ViewModel.CreateNewEmailCommand.Execute(hyperlink.DataContext);
        }
    }
}
