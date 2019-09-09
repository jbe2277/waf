using Autofac;

namespace Waf.NewsReader.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(((App)App.Current).Container.Resolve<Presentation.App>());
        }
    }
}
