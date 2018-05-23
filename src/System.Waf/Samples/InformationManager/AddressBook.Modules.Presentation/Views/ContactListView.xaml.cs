using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.Views
{
    [Export(typeof(IContactListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ContactListView : UserControl, IContactListView
    {
        private readonly Lazy<ContactListViewModel> viewModel;
        private ICollectionView contactCollectionView;
        
        public ContactListView()
        {
            InitializeComponent();
            viewModel = new Lazy<ContactListViewModel>(() => ViewHelper.GetViewModel<ContactListViewModel>(this));
            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        private ContactListViewModel ViewModel => viewModel.Value;

        public void FocusItem()
        {
            if (ViewModel.SelectedContact == null)
            {
                contactsBox.Focus();
                return;
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() => {
                // It is necessary to delay this code because data binding updates the values asynchronously.
                contactsBox.ScrollIntoView(ViewModel.SelectedContact);
                var selectedListBoxItem = (ListBoxItem)contactsBox.ItemContainerGenerator.ContainerFromItem(ViewModel.SelectedContact);
                selectedListBoxItem?.Focus();
            }));
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            // The following code doesn't work in the WPF Designer environment.
            if (!WafConfiguration.IsInDesignMode)
            {
                contactCollectionView = CollectionViewSource.GetDefaultView(ViewModel.Contacts);
                contactCollectionView.Filter = Filter;
                ViewModel.ContactCollectionView = contactCollectionView.Cast<Contact>();
                ViewModel.SelectedContact = ViewModel.ContactCollectionView.FirstOrDefault();
            }

            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            contactsBox.Focus();
            if (ViewModel.SelectedContact != null)
            {
                FocusItem();
            }
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            // We need to set the Filter back to null => prevent a WPF memory leak
            contactCollectionView.Filter = null;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
        }

        private bool Filter(object obj)
        {
            return ViewModel.Filter((Contact)obj);
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.FilterText))
            {
                contactCollectionView.Refresh();
            }
        }
    }
}
