using Waf.NewsReader.Applications.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Waf.NewsReader.Presentation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : TabbedPage, ISettingsView
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        public object? DataContext
        {
            get => BindingContext;
            set => BindingContext = value;
        }
    }
}