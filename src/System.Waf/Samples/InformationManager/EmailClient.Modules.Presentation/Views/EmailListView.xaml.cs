using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using System.Windows.Threading;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views
{
    [Export(typeof(IEmailListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class EmailListView : IEmailListView
    {
        private readonly Lazy<EmailListViewModel> viewModel;
        
        public EmailListView()
        {
            InitializeComponent();
            viewModel = new Lazy<EmailListViewModel>(() => this.GetViewModel<EmailListViewModel>()!);
            Loaded += LoadedHandler;
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
            ViewModel.SelectedEmail = ViewModel.Emails.FirstOrDefault();
            emailsBox.Focus();
            if (ViewModel.SelectedEmail != null)
            {
                FocusItem();
            }
        }
    }
}
