using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

public class ExchangeSettingsViewModel : ViewModel<IExchangeSettingsView>
{
    public ExchangeSettingsViewModel(IExchangeSettingsView view) : base(view)
    {
    }

    public ExchangeSettings? Model { get; set; }
}
