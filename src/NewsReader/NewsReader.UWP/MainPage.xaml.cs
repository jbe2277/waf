namespace Waf.NewsReader.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new NewsReader.Presentation.App());
        }
    }
}
