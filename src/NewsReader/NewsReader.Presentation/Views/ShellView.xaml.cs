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
        private ShellViewModel viewModel = null!;
        private bool isFirstPage = true;

        public ShellView()
        {
            InitializeComponent();
        }

        public object? DataContext
        {
            get => BindingContext;
            set => BindingContext = value;
        }

        public async Task PushAsync(object page)
        {
            bool wasFirstPage = isFirstPage;
            isFirstPage = false;
            var navi = Detail.Navigation;
            var idx = navi.NavigationStack.IndexOf(page);
            if (idx >= 0)
            {
                if (idx == navi.NavigationStack.Count - 1) return;
                for (int i = 0; i < navi.NavigationStack.Count - idx - 2; i++) navi.RemovePage(navi.NavigationStack[navi.NavigationStack.Count - 2]);
                await navi.PopAsync();
            }
            else await navi.PushAsync((Page)page);
            if (wasFirstPage) navi.RemovePage(navi.NavigationStack[0]);  // Remove initial empty page from navigation stack
        }

        public Task PopAsync()
        {
            return Detail.Navigation.PopAsync();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            viewModel = (ShellViewModel)BindingContext;
            var navigationPage = new NavigationPage(new Page());  // Add empty page; needed by MasterDetailPage when shown
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