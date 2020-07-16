using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Waf.NewsReader.Presentation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditFeedView : IAddEditFeedView
    {
        private AddEditFeedViewModel viewModel = null!;

        public AddEditFeedView()
        {
            InitializeComponent();
        }

        public object? DataContext
        {
            get => BindingContext;
            set => BindingContext = value;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            viewModel = (AddEditFeedViewModel)BindingContext;
        }

        private void FeedUrlUnfocused(object sender, FocusEventArgs e)
        {
            viewModel.LoadFeedCommand.Execute(null);
        }
    }
}