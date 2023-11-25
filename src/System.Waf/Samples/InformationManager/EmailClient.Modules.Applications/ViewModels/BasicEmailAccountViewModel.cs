using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

[Export, PartCreationPolicy(CreationPolicy.NonShared)]
public class BasicEmailAccountViewModel : ViewModel<IBasicEmailAccountView>
{
    private EmailAccount emailAccount = null!;
    private bool isPop3Checked = true;
    private bool isExchangeChecked;

    [ImportingConstructor]
    public BasicEmailAccountViewModel(IBasicEmailAccountView view) : base(view)
    {
    }

    public EmailAccount EmailAccount
    {
        get => emailAccount;
        set
        {
            if (emailAccount == value) return;
            emailAccount = value;
            if (emailAccount.EmailAccountSettings is Pop3Settings) IsPop3Checked = true;
            else if (emailAccount.EmailAccountSettings is ExchangeSettings) IsExchangeChecked = true;
            RaisePropertyChanged();
        }
    }

    public bool IsPop3Checked
    {
        get => isPop3Checked;
        set
        {
            if (!SetProperty(ref isPop3Checked, value)) return;
            IsExchangeChecked = !value;
        }
    }

    public bool IsExchangeChecked
    {
        get => isExchangeChecked;
        set
        {
            if (!SetProperty(ref isExchangeChecked, value)) return;
            IsPop3Checked = !value;
        }
    }
}
