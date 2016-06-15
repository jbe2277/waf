using Jbe.NewsReader.Applications.Views;
using System.Composition;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IInfoSettingsView)), Shared]
    public sealed partial class InfoSettingsView : UserControl, IInfoSettingsView
    {
        public InfoSettingsView()
        {
            this.InitializeComponent();
        }
    }
}
