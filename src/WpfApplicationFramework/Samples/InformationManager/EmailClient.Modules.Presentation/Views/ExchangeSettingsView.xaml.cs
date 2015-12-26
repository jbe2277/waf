using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views
{
    [Export(typeof(IExchangeSettingsView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ExchangeSettingsView : UserControl, IExchangeSettingsView
    {
        public ExchangeSettingsView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(LoadedHandler);
        }


        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            serverPathBox.Focus();
        }
    }
}
