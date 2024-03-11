using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Controllers;

/// <summary>Responsible for a email folder. Synchronizes the master / detail view.</summary>
[Export, PartCreationPolicy(CreationPolicy.NonShared)]
internal class EmailFolderController
{
    private readonly IShellService shellService;
    private readonly EmailLayoutViewModel emailLayoutViewModel;
    private readonly DelegateCommand deleteEmailCommand;
    private ObservableListViewCore<Email> emailsView = null!;
    private IWeakEventProxy? emailListViewModelPropertyChangedProxy;

    [ImportingConstructor]
    public EmailFolderController(IShellService shellService, EmailLayoutViewModel emailLayoutViewModel, EmailListViewModel emailListViewModel, EmailViewModel emailViewModel)
    {
        this.shellService = shellService;
        this.emailLayoutViewModel = emailLayoutViewModel;
        EmailListViewModel = emailListViewModel;
        EmailViewModel = emailViewModel;
        deleteEmailCommand = new(DeleteEmail, CanDeleteEmail);
    }

    public EmailFolder EmailFolder { get; set; } = null!;

    public ICommand DeleteEmailCommand => deleteEmailCommand;

    internal EmailListViewModel EmailListViewModel { get; }

    internal EmailViewModel EmailViewModel { get; }

    public void Initialize()
    {
        emailsView = new(EmailFolder.Emails, null, EmailListViewModel.Filter, x => x.OrderByDescending(y => y.Sent));
        EmailListViewModel.Emails = emailsView;
        EmailListViewModel.DeleteEmailCommand = DeleteEmailCommand;
        emailListViewModelPropertyChangedProxy = WeakEvent.PropertyChanged.Add(EmailListViewModel, EmailListViewModelPropertyChanged);
        emailLayoutViewModel.EmailListView = EmailListViewModel.View;
        emailLayoutViewModel.EmailView = EmailViewModel.View;
    }

    public void Run() => shellService.ContentView = emailLayoutViewModel.View;

    public void Shutdown()
    {
        WeakEvent.TryRemove(ref emailListViewModelPropertyChangedProxy);
        // Set the views to null so that the garbage collector can collect them.
        emailLayoutViewModel.EmailListView = null;
        emailLayoutViewModel.EmailView = null;
        emailsView.Dispose();
    }

    private bool CanDeleteEmail() => EmailListViewModel.SelectedEmail != null;

    private void DeleteEmail()
    {
        var nextEmail = EmailListViewModel.Emails.GetNextElementOrDefault(EmailListViewModel.SelectedEmail);
        EmailFolder.RemoveEmail(EmailListViewModel.SelectedEmail!);
        EmailListViewModel.SelectedEmail = nextEmail ?? EmailListViewModel.Emails.LastOrDefault();
        EmailListViewModel.FocusItem();
    }

    private void EmailListViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
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
