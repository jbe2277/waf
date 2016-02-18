using System.ComponentModel.Composition;
using System.Windows;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Presentation.Views
{
    [Export(typeof(ISaveChangesView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SaveChangesWindow : Window, ISaveChangesView
    {
        public SaveChangesWindow()
        {
            InitializeComponent();
        }


        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }
    }
}
