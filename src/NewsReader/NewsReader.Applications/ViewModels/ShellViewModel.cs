using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels;

public class ShellViewModel : ViewModelCore<IShellView>, INavigationService
{
    private NavigationItem? selectedFooterMenu;
    private ObservableCollection<Feed> feeds = null!;
    private Feed? selectedFeed;

    public ShellViewModel(IShellView view, IAppInfoService appInfoService) : base(view, false)
    {
        AppName = appInfoService.AppName;
    }

    public string AppName { get; }

    public ICommand EditFeedCommand { get; internal set; } = null!;

    public ICommand RemoveFeedCommand { get; internal set; } = null!;

    public ICommand ShowFeedViewCommand { get; internal set; } = null!;

    public IReadOnlyList<NavigationItem> FooterMenu { get; internal set; } = null!;

    public NavigationItem? SelectedFooterMenu
    {
        get => selectedFooterMenu;
        set
        {
            if (!SetProperty(ref selectedFooterMenu, value)) return;
            if (selectedFooterMenu is not null)
            {
                SelectedFeed = null;
                selectedFooterMenu.Command?.Execute(null);
            }
        }
    }

    public ObservableCollection<Feed> Feeds
    {
        get => feeds;
        internal set => SetProperty(ref feeds, value);
    }

    public Feed? SelectedFeed
    {
        get => selectedFeed;
        set
        {
            if (!SetProperty(ref selectedFeed, value)) return;
            if (selectedFeed is not null)
            {
                SelectedFooterMenu = null;
                ShowFeedViewCommand.Execute(selectedFeed);
            }
        }
    }

    public Task Navigate(IViewModelCore viewModel)
    {
        ViewCore.CloseFlyout();
        viewModel.Initialize();
        return ViewCore.PushAsync(viewModel.View);
    }

    public Task NavigateBack() => ViewCore.PopAsync();
}
