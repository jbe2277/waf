using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels;

public class ShellViewModel(IShellView view, IAppInfoService appInfoService) : ViewModelCore<IShellView>(view, false), INavigationService
{
    private bool suppressSelectedFooterMenuCommand;
    private bool suppressSelectedFeedCommand;

    public string AppName { get; } = appInfoService.AppName;

    public ICommand EditFeedCommand { get; internal set; } = null!;

    public ICommand RemoveFeedCommand { get; internal set; } = null!;

    public ICommand ShowFeedViewCommand { get; internal set; } = null!;

    public IReadOnlyList<NavigationItem> FooterMenu { get; internal set; } = null!;

    public object? CurrentPage { get; private set => SetProperty(ref field, value); }

    public NavigationItem? SelectedFooterMenu
    {
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            if (field is not null)
            {
                SelectedFeed = null;
                if (!suppressSelectedFooterMenuCommand) field.Command?.Execute(null);
            }
        }
    }

    public ObservableList<Feed> Feeds { get; internal set => SetProperty(ref field, value); } = null!;

    public Feed? SelectedFeed
    {
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            if (field is not null)
            {
                SelectedFooterMenu = null;
                if (!suppressSelectedFeedCommand) ShowFeedViewCommand.Execute(field);
            }
        }
    }

    public void SetSelectedFooterMenuCore(NavigationItem? value)
    {
        suppressSelectedFooterMenuCommand = true;
        SelectedFooterMenu = value;
        suppressSelectedFooterMenuCommand = false;
    }

    public void SetSelectedFeedCore(Feed? value)
    {
        suppressSelectedFeedCommand = true;
        SelectedFeed = value;
        suppressSelectedFeedCommand = false;
    }

    public Task Navigate(IViewModelCore viewModel)
    {
        ViewCore.CloseFlyout();
        viewModel.Initialize();
        return ViewCore.PushAsync(viewModel.View);
    }

    public Task NavigateBack() => ViewCore.PopAsync();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void InternalSetCurrentPage(object? currentPage) => CurrentPage = currentPage;
}
