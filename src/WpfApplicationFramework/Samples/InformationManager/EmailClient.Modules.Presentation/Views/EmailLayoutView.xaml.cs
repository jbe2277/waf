using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
