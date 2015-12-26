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
        private readonly EmailListViewModel emailListViewModel;
        private readonly EmailViewModel emailViewModel;
        private readonly DelegateCommand deleteEmailCommand;

        
        [ImportingConstructor]
        public EmailFolderController(IShellService shellService, EmailLayoutViewModel emailLayoutViewModel, EmailListViewModel emailListViewModel, EmailViewModel emailViewModel)
        {
            this.shellService = shellService;
            this.emailLayoutViewModel = emailLayoutViewModel;
            this.emailListViewModel = emailListViewModel;
            this.emailViewModel = emailViewModel;
            this.deleteEmailCommand = new DelegateCommand(DeleteEmail, CanDeleteEmail);
        }

        
        public EmailFolder EmailFolder { get; set; }

        public ICommand DeleteEmailCommand { get { return deleteEmailCommand; } }

        internal EmailListViewModel EmailListViewModel { get { return emailListViewModel; } }

        internal EmailViewModel EmailViewModel { get { return emailViewModel; } }


        public void Initialize()
        {
            emailListViewModel.Emails = EmailFolder.Emails;
            emailListViewModel.DeleteEmailCommand = DeleteEmailCommand;

            PropertyChangedEventManager.AddHandler(emailListViewModel, EmailListViewModelPropertyChanged, "");

            emailLayoutViewModel.EmailListView = emailListViewModel.View;
            emailLayoutViewModel.EmailView = emailViewModel.View;
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

        private bool CanDeleteEmail() { return emailListViewModel.SelectedEmail != null; }

        private void DeleteEmail()
        {
            // Use the EmailCollectionView, which represents the sorted/filtered state of the emails, to determine the next email to select.
            var nextEmail = CollectionHelper.GetNextElementOrDefault(emailListViewModel.EmailCollectionView, emailListViewModel.SelectedEmail);
            
            EmailFolder.RemoveEmail(emailListViewModel.SelectedEmail);

            emailListViewModel.SelectedEmail = nextEmail ?? emailListViewModel.EmailCollectionView.LastOrDefault();
            emailListViewModel.FocusItem();
        }

        private void EmailListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedEmail")
            {
                emailViewModel.Email = emailListViewModel.SelectedEmail;
                deleteEmailCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
