using Jbe.NewsReader.Applications.Views;
using System.Composition;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(ISettingsLayoutView)), Shared]
    public sealed partial class SettingsLayoutView : UserControl, ISettingsLayoutView
    {
        public SettingsLayoutView()
        {
            this.InitializeComponent();
        }
    }
}
