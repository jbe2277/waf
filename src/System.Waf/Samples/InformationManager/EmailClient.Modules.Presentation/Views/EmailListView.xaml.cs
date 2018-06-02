using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using System.Windows.Threading;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views
{
    [Export(typeof(IEmailListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class EmailListView : UserControl, IEmailListView
    {
        private readonly Lazy<EmailListViewModel> viewModel;
        private ICollectionView emailCollectionView;
        
        public EmailListView()
        {
            InitializeComponent();
            viewModel = new Lazy<EmailListViewModel>(() => ViewHelper.GetViewModel<EmailListViewModel>(this));
            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        private EmailListViewModel ViewModel => viewModel.Value;

        public void FocusItem()
        {
            if (ViewModel.SelectedEmail == null)
            {
                emailsBox.Focus();
                return;
            }
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() =>
            {
                // It is necessary to delay this code because data binding updates the values asynchronously.
                emailsBox.ScrollIntoView(ViewModel.SelectedEmail);
                var selectedListBoxItem = (ListBoxItem)emailsBox.ItemContainerGenerator.ContainerFromItem(ViewModel.SelectedEmail);
                selectedListBoxItem?.Focus();
            }));
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            // The following code doesn't work in the WPF Designer environment (Cider or Blend).
            if (!WafConfiguration.IsInDesignMode)
            {
                emailCollectionView = CollectionViewSource.GetDefaultView(ViewModel.Emails);
                emailCollectionView.Filter = Filter;
                emailCollectionView.SortDescriptions.Add(new SortDescription(nameof(Email.Sent), ListSortDirection.Descending));
                ViewModel.EmailCollectionView = emailCollectionView.Cast<Email>();
                ViewModel.SelectedEmail = ViewModel.EmailCollectionView.FirstOrDefault();
            }
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            emailsBox.Focus();
            if (ViewModel.SelectedEmail != null)
            {
                FocusItem();
            }
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            // We need to set the Filter back to null => prevent a WPF memory leak
            emailCollectionView.Filter = null;
            emailCollectionView.SortDescriptions.Clear();
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
        }

        private bool Filter(object obj)
        {
            return ViewModel.Filter((Email)obj);
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.FilterText))
            {
                emailCollectionView.Refresh();
            }
        }
    }
}
