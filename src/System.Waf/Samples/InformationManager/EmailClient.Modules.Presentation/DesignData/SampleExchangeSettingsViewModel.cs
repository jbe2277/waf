using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData;

public class SampleExchangeSettingsViewModel : ExchangeSettingsViewModel
{
    public SampleExchangeSettingsViewModel() : base(new MockExchangeSettingsView())
    {
        Model = new ExchangeSettings() { ServerPath = "exchange.example.com", UserName = "User42" };
    }


    private sealed class MockExchangeSettingsView : IExchangeSettingsView
    {
        public object? DataContext { get; set; }
    }
}
