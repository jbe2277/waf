using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

public class BasicEmailAccountViewModel(IBasicEmailAccountView view) : ViewModel<IBasicEmailAccountView>(view)
{
    public EmailAccount EmailAccount
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            if (field.EmailAccountSettings is Pop3Settings) IsPop3Checked = true;
            else if (field.EmailAccountSettings is ExchangeSettings) IsExchangeChecked = true;
            RaisePropertyChanged();
        }
    } = null!;

    public bool IsPop3Checked
    {
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            IsExchangeChecked = !value;
        }
    } = true;

    public bool IsExchangeChecked
    {
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            IsPop3Checked = !value;
        }
    }
}
