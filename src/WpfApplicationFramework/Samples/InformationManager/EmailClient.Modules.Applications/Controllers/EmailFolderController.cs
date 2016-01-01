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

        
        [ImportingConstructor]
        public EmailFolderController(IShellService shellService, EmailLayoutViewModel emailLayoutViewModel, EmailListViewModel emailListViewModel, EmailViewModel emailViewModel)
        {
            this.shellService = shellService;
            this.emailLayoutViewModel = emailLayoutViewModel;
            this.EmailListViewModel = emailListViewModel;
            this.EmailViewModel = emailViewModel;
            this.deleteEmailCommand = new DelegateCommand(DeleteEmail, CanDeleteEmail);
        }

        
        public EmailFolder EmailFolder { get; set; }

        public ICommand DeleteEmailCommand => deleteEmailCommand;

        internal EmailListViewModel EmailListViewModel { get; }

        internal EmailViewModel EmailViewModel { get; }


        public void Initialize()
        {
            EmailListViewModel.Emails = EmailFolder.Emails;
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
            // Set the views to null so that the garbage collector can collect them.
            emailLayoutViewModel.EmailListView = null;
            emailLayoutViewModel.EmailView = null;
        }

        private bool CanDeleteEmail() { return EmailListViewModel.SelectedEmail != null; }

        private void DeleteEmail()
        {
            // Use the EmailCollectionView, which represents the sorted/filtered state of the emails, to determine the next email to select.
            var nextEmail = CollectionHelper.GetNextElementOrDefault(EmailListViewModel.EmailCollectionView, EmailListViewModel.SelectedEmail);
            
            EmailFolder.RemoveEmail(EmailListViewModel.SelectedEmail);

            EmailListViewModel.SelectedEmail = nextEmail ?? EmailListViewModel.EmailCollectionView.LastOrDefault();
            EmailListViewModel.FocusItem();
        }

        private void EmailListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EmailListViewModel.SelectedEmail))
            {
                EmailViewModel.Email = EmailListViewModel.SelectedEmail;
                deleteEmailCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
