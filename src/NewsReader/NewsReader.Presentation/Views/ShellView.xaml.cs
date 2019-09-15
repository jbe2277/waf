using System.Threading.Tasks;
using System.Waf.Foundation;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Waf.NewsReader.Presentation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellView : IShellView
    {
        private ShellViewModel viewModel;

        public ShellView()
        {
            InitializeComponent();
        }

        public object DataContext
        {
            get => BindingContext;
            set => BindingContext = value;
        }

        public async Task PushAsync(object page)
        {
            var navi = Detail.Navigation;
            var idx = navi.NavigationStack.IndexOf(page);
            if (idx >= 0)
            {
                if (idx == navi.NavigationStack.Count - 1) return;
                for (int i = 0; i < navi.NavigationStack.Count - idx - 2; i++) navi.RemovePage(navi.NavigationStack[navi.NavigationStack.Count - 2]);
                await navi.PopAsync();
            }
            else await navi.PushAsync((Page)page);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            viewModel = (ShellViewModel)BindingContext;
            // TODO: start
            // viewModel.StartViewModel.Initialize();
            // var navigationPage = new NavigationPage((Page)viewModel.StartViewModel.View);
            var navigationPage = new NavigationPage(new Page() { Title = "TODO" });
            Detail = navigationPage;
        }

        private void NavigationItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is NavigationItem item)
            {
                if (item.Command != null)
                {
                    item.Command.Execute(null);
                    if (MasterBehavior != MasterBehavior.Split) IsPresented = false;
                }
            }
        }

        private void FeedsItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.ShowFeedViewCommand.Execute(e.Item);
            if (MasterBehavior != MasterBehavior.Split) IsPresented = false;
        }
    }
}