using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.Views;

[Export(typeof(IContactListView)), PartCreationPolicy(CreationPolicy.NonShared)]
public partial class ContactListView : IContactListView
{
    private readonly Lazy<ContactListViewModel> viewModel;

    public ContactListView()
    {
        InitializeComponent();
        viewModel = new(() => ViewHelper.GetViewModel<ContactListViewModel>(this)!);
        Loaded += LoadedHandler;
    }

    private ContactListViewModel ViewModel => viewModel.Value;

    public void FocusItem()
    {
        if (ViewModel.SelectedContact == null)
        {
            contactsBox.Focus();
            return;
        }

        Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
        {
            // It is necessary to delay this code because data binding updates the values asynchronously.
            contactsBox.ScrollIntoView(ViewModel.SelectedContact);
            var selectedListBoxItem = (ListBoxItem)contactsBox.ItemContainerGenerator.ContainerFromItem(ViewModel.SelectedContact);
            selectedListBoxItem?.Focus();
        });
    }

    private void LoadedHandler(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectedContact = ViewModel.Contacts.FirstOrDefault();
        contactsBox.Focus();
        if (ViewModel.SelectedContact != null) FocusItem();
    }
}
