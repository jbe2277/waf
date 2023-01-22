using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications.Views;

namespace Waf.NewsReader.Presentation.Views;

public partial class ShellView : IShellView
{
    private ShellViewModel viewModel = null!;
    private bool isFirstPage = true;

    public ShellView()
    {
        InitializeComponent();
    }

    public async Task PushAsync(object page)
    {
        bool wasFirstPage = isFirstPage;
        isFirstPage = false;
        var navi = Detail.Navigation;
        var idx = navi.NavigationStack.IndexOf(page);
        if (idx >= 0)
        {
            // Pushing of a page which already exists in the navigation stack is not allowed -> InvalidOperationException: 'Page must not already have a parent.'
            // If the specified page already exists in the navigaton stack then remove all pages after the page and pop to it.
            if (idx == navi.NavigationStack.Count - 1) return;
            for (int i = 0; i < navi.NavigationStack.Count - idx - 2; i++) navi.RemovePage(navi.NavigationStack[navi.NavigationStack.Count - 2]);
            await navi.PopAsync();
        }
        else await navi.PushAsync((Page)page);
        if (wasFirstPage) navi.RemovePage(navi.NavigationStack[0]);  // Remove initial empty page from navigation stack
    }

    public Task PopAsync() => Detail.Navigation.PopAsync();

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        viewModel = (ShellViewModel)BindingContext;
    }

    private void NavigationItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is NavigationItem item)
        {
            if (item.Command != null)
            {
                item.Command.Execute(null);
                CloseFlyout();
            }
        }
    }

    private void FeedsItemTapped(object sender, ItemTappedEventArgs e)
    {
        viewModel.ShowFeedViewCommand.Execute(e.Item);
        CloseFlyout();
    }

    private void CloseFlyout()
    {
        if (!((IFlyoutPageController)this).ShouldShowSplitMode) IsPresented = false;
    }
}