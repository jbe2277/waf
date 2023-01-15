using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications.Views;

namespace Waf.NewsReader.Presentation.Views;

public partial class FeedView : IFeedView
{
    private FeedViewModel viewModel = null!;

    public FeedView()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        viewModel = (FeedViewModel)BindingContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        searchBar.IsVisible = false;
        viewModel.SearchText = "";
    }

    private void SearchClicked(object sender, EventArgs e)
    {
        searchBar.IsVisible = !searchBar.IsVisible;
        if (searchBar.IsVisible) searchBar.Focus();
    }
}