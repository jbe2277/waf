using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Controllers;

/// <summary>Responsible for an email account.</summary>
internal class EmailAccountsController
{
    private readonly IShellService shellService;
    private readonly Func<EditEmailAccountController> editEmailAccountControllerFactory;
    private readonly Func<EmailAccountsViewModel> emailAccountsViewModelFactory;
    private DelegateCommand? removeEmailAccountCommand;
    private DelegateCommand? editEmailAccountCommand;
    private EmailAccountsViewModel emailAccountsViewModel = null!;

    public EmailAccountsController(IShellService shellService, Func<EditEmailAccountController> editEmailAccountControllerFactory, Func<EmailAccountsViewModel> emailAccountsViewModelFactory)
    {
        this.shellService = shellService;
        this.editEmailAccountControllerFactory = editEmailAccountControllerFactory;
        this.emailAccountsViewModelFactory = emailAccountsViewModelFactory;
        EmailAccountsCommand = new DelegateCommand(ShowEmailAccounts);
    }

    public EmailClientRoot Root { get; set; } = null!;

    public ICommand EmailAccountsCommand { get; }

    private void ShowEmailAccounts()
    {
        removeEmailAccountCommand = new(RemoveEmailAccount, CanRemoveEmailAccount);
        editEmailAccountCommand = new(EditEmailAccount, CanEditEmailAccount);

        emailAccountsViewModel = emailAccountsViewModelFactory();
        emailAccountsViewModel.EmailClientRoot = Root;
        emailAccountsViewModel.SelectedEmailAccount = Root.EmailAccounts.FirstOrDefault();
        emailAccountsViewModel.NewAccountCommand = new DelegateCommand(NewEmailAccount);
        emailAccountsViewModel.RemoveAccountCommand = removeEmailAccountCommand;
        emailAccountsViewModel.EditAccountCommand = editEmailAccountCommand;
        emailAccountsViewModel.PropertyChanged += EmailAccountsViewModelPropertyChanged;

        emailAccountsViewModel.ShowDialog(shellService.ShellView);

        emailAccountsViewModel.PropertyChanged -= EmailAccountsViewModelPropertyChanged;
        emailAccountsViewModel = null!;
        removeEmailAccountCommand = null;
    }

    private void NewEmailAccount()
    {
        var editEmailAccountController = editEmailAccountControllerFactory();
        editEmailAccountController.OwnerWindow = emailAccountsViewModel.View;
        var emailAccount = new EmailAccount();
        emailAccount.Validate();
        editEmailAccountController.EmailAccount = emailAccount;

        editEmailAccountController.Initialize();
        if (editEmailAccountController.Run())
        {
            Root.AddEmailAccount(editEmailAccountController.EmailAccount);
        }
    }

    private bool CanRemoveEmailAccount() => emailAccountsViewModel.SelectedEmailAccount != null;

    private void RemoveEmailAccount() => Root.RemoveEmailAccount(emailAccountsViewModel.SelectedEmailAccount!);

    private bool CanEditEmailAccount() => emailAccountsViewModel.SelectedEmailAccount != null;

    private void EditEmailAccount()
    {
        var originalAccount = emailAccountsViewModel.SelectedEmailAccount!;
        var editEmailAccountController = editEmailAccountControllerFactory();
        editEmailAccountController.OwnerWindow = emailAccountsViewModel.View;
        editEmailAccountController.EmailAccount = originalAccount.Clone();
        editEmailAccountController.Initialize();
        if (editEmailAccountController.Run())
        {
            Root.ReplaceEmailAccount(originalAccount, editEmailAccountController.EmailAccount);
        }
    }

    private void EmailAccountsViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(EmailAccountsViewModel.SelectedEmailAccount))
        {
            DelegateCommand.RaiseCanExecuteChanged(removeEmailAccountCommand!, editEmailAccountCommand!);
        }
    }
}
