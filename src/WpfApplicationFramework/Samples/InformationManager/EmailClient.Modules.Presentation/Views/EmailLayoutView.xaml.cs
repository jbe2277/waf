using System.Windows.Controls;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using System.ComponentModel.Composition;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views
{
    [Export(typeof(IEmailLayoutView))]
    public partial class EmailLayoutView : UserControl, IEmailLayoutView
    {
        public EmailLayoutView()
        {
            InitializeComponent();
        }
    }
}
