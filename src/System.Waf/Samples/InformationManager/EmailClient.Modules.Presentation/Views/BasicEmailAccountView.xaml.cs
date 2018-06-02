using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views
{
    [Export(typeof(IBasicEmailAccountView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class BasicEmailAccountView : UserControl, IBasicEmailAccountView
    {
        public BasicEmailAccountView()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            nameBox.Focus();
        }
    }
}
