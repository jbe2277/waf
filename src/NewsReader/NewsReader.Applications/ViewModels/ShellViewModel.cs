using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels;

public class ShellViewModel(IShellView view, IAppInfoService appInfoService) : ViewModelCore<IShellView>(view, false), INavigationService
{
    private NavigationItem? selectedFooterMenu;
    private ObservableList<Feed> feeds = null!;
    private Feed? selectedFeed;
    private object? currentPage;
    private bool suppressSelectedFooterMenuCommand;
    private bool suppressSelectedFeedCommand;

    public string AppName { get; } = appInfoService.AppName;

    public ICommand EditFeedCommand { get; internal set; } = null!;

    public ICommand RemoveFeedCommand { get; internal set; } = null!;

    public ICommand ShowFeedViewCommand { get; internal set; } = null!;

    public IReadOnlyList<NavigationItem> FooterMenu { get; internal set; } = null!;

    public object? CurrentPage { get => currentPage; private set => SetProperty(ref currentPage, value); }

    public NavigationItem? SelectedFooterMenu
    {
        get => selectedFooterMenu;
        set
        {
            if (!SetProperty(ref selectedFooterMenu, value)) return;
            if (selectedFooterMenu is not null)
            {
                SelectedFeed = null;  // TODO: CollectionView shows this still as selected! Also initial Feed selection not shown (Windows)
                if (!suppressSelectedFooterMenuCommand) selectedFooterMenu.Command?.Execute(null);
            }
        }
    }

    public ObservableList<Feed> Feeds { get => feeds; internal set => SetProperty(ref feeds, value); }

    public Feed? SelectedFeed
    {
        get => selectedFeed;
        set
        {
            if (!SetProperty(ref selectedFeed, value)) return;
            if (selectedFeed is not null)
            {
                SelectedFooterMenu = null;
                if (!suppressSelectedFeedCommand) ShowFeedViewCommand.Execute(selectedFeed);
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
