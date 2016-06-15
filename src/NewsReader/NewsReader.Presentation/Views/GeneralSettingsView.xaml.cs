using Jbe.NewsReader.Applications.Views;
using System.Composition;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IGeneralSettingsView)), Shared]
    public sealed partial class GeneralSettingsView : UserControl, IGeneralSettingsView
    {
        public GeneralSettingsView()
        {
            this.InitializeComponent();
        }
    }
}
