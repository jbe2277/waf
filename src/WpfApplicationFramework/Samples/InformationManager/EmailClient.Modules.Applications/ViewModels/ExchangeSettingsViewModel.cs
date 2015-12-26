using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ExchangeSettingsViewModel : ViewModel<IExchangeSettingsView>
    {
        private ExchangeSettings model;

        
        [ImportingConstructor]
        public ExchangeSettingsViewModel(IExchangeSettingsView view) : base(view)
        {
        }


        public ExchangeSettings Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
    }
}
