using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Controllers
{
    /// <summary>
    /// Responsible for a email folder. Synchronizes the master / detail view.
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class EmailFolderController
    {
        private readonly IShellService shellService;
        private readonly EmailLayoutViewModel emailLayoutViewModel;
        private readonly DelegateCommand deleteEmailCommand;
        private ObservableListView<Email> emailsView;

        [ImportingConstructor]
        public EmailFolderController(IShellService shellService, EmailLayoutViewModel emailLayoutViewModel, EmailListViewModel emailListViewModel, EmailViewModel emailViewModel)
        {
            this.shellService = shellService;
            this.emailLayoutViewModel = emailLayoutViewModel;
            EmailListViewModel = emailListViewModel;
            EmailViewModel = emailViewModel;
            deleteEmailCommand = new DelegateCommand(DeleteEmail, CanDeleteEmail);
        }

        public EmailFolder EmailFolder { get; set; }

        public ICommand DeleteEmailCommand => deleteEmailCommand;

        internal EmailListViewModel EmailListViewModel { get; }

        internal EmailViewModel EmailViewModel { get; }

        public void Initialize()
        {
            emailsView = new ObservableListView<Email>(EmailFolder.Emails, filter: EmailListViewModel.Filter, sort: x => x.OrderByDescending(y => y.Sent));
            EmailListViewModel.Emails = emailsView;
            EmailListViewModel.DeleteEmailCommand = DeleteEmailCommand;
            PropertyChangedEventManager.AddHandler(EmailListViewModel, EmailListViewModelPropertyChanged, "");
            emailLayoutViewModel.EmailListView = EmailListViewModel.View;
            emailLayoutViewModel.EmailView = EmailViewModel.View;
        }

        public void Run()
        {
            shellService.ContentView = emailLayoutViewModel.View;
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(EmailListViewModel, EmailListViewModelPropertyChanged, "");
            // Set the views to null so that the garbage collector can collect them.
            emailLayoutViewModel.EmailListView = null;
            emailLayoutViewModel.EmailView = null;
            emailsView.Dispose();
        }

        private bool CanDeleteEmail() { return EmailListViewModel.SelectedEmail != null; }

        private void DeleteEmail()
        {
            var nextEmail = CollectionHelper.GetNextElementOrDefault(EmailListViewModel.Emails, EmailListViewModel.SelectedEmail);
            EmailFolder.RemoveEmail(EmailListViewModel.SelectedEmail);
            EmailListViewModel.SelectedEmail = nextEmail ?? EmailListViewModel.Emails.LastOrDefault();
            EmailListViewModel.FocusItem();
        }

        private void EmailListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EmailListViewModel.SelectedEmail))
            {
                EmailViewModel.Email = EmailListViewModel.SelectedEmail;
                deleteEmailCommand.RaiseCanExecuteChanged();
            }
            else if (e.PropertyName == nameof(EmailListViewModel.FilterText))
            {
                emailsView.Update();
            }
        }
    }
}
