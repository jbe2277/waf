using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications.Views;

namespace Waf.NewsReader.Presentation.Views;

public partial class ShellView : IShellView
{
    private ShellViewModel? viewModel;
    private bool isFirstPage = true;

    public ShellView()
    {
        InitializeComponent();
        navigationPage.Pushed += Navigated;
        navigationPage.Popped += Navigated;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (viewModel is not null) return;
        viewModel = (ShellViewModel)BindingContext;
    }

    public async Task PushAsync(object page)
    {
        bool wasFirstPage = isFirstPage;
        isFirstPage = false;
        var navi = navigationPage.Navigation;
        var idx = navi.NavigationStack.IndexOf(page);
        if (idx >= 0)
        {
            // Pushing of a page which already exists in the navigation stack is not allowed -> InvalidOperationException: 'Page must not already have a parent.'
            // If the specified page already exists in the navigaton stack then remove all pages after the page and pop to it.
            if (idx == navi.NavigationStack.Count - 1) return;
            for (int i = 0; i < navi.NavigationStack.Count - idx - 2; i++) navi.RemovePage(navi.NavigationStack[^2]);
            await navi.PopAsync();
        }
        else await navi.PushAsync((Page)page);
        if (wasFirstPage) navi.RemovePage(navi.NavigationStack[0]);  // Remove initial empty page from navigation stack
    }

    public Task PopAsync() => navigationPage.Navigation.PopAsync();

    public void CloseFlyout()
    {
        if (!((IFlyoutPageController)this).ShouldShowSplitMode) IsPresented = false;
    }

    private void Navigated(object? sender, NavigationEventArgs e)
    {
        viewModel?.InternalSetCurrentPage(navigationPage.CurrentPage);
    }
}