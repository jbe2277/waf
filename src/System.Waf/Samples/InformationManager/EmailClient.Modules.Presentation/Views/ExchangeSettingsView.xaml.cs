using System.ComponentModel.Composition;
using System.Windows;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views
{
    [Export(typeof(IExchangeSettingsView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ExchangeSettingsView : IExchangeSettingsView
    {
        public ExchangeSettingsView()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e) => serverPathBox.Focus();
    }
}
