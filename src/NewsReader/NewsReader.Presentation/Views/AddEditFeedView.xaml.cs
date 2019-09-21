using Waf.NewsReader.Applications.Views;
using Xamarin.Forms.Xaml;

namespace Waf.NewsReader.Presentation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditFeedView : IAddEditFeedView
    {
        public AddEditFeedView()
        {
            InitializeComponent();
        }

        public object DataContext
        {
            get => BindingContext;
            set => BindingContext = value;
        }
    }
}