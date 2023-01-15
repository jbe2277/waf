using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications.Views;

namespace Waf.NewsReader.Presentation.Views;

public partial class AddEditFeedView : IAddEditFeedView
{
    private AddEditFeedViewModel viewModel = null!;

    public AddEditFeedView()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        viewModel = (AddEditFeedViewModel)BindingContext;
    }

    private void FeedUrlUnfocused(object sender, FocusEventArgs e) => viewModel.LoadFeedCommand.Execute(null);
}